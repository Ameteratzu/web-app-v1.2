using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactos;
public class ManageImpactosCommandHandler : IRequestHandler<ManageImpactosCommand, ManageImpactoResponse>
{
    private readonly ILogger<CreateImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageImpactosCommandHandler(
        ILogger<CreateImpactoEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageImpactoResponse> Handle(ManageImpactosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateImpactoEvolucionCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateImpactosClasificadosAsync(request);
        await ValidateTiposDanioAsync(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Evolucion>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Evolucion);

            var evolucion = await GetOrCreateEvolucion(request, registroActualizacion);

            var impactosOriginales = evolucion.Impactos.ToDictionary(d => d.Id, d => _mapper.Map<ManageImpactoDto>(d));
            MapAndManageImpactos(request, evolucion);

            var impactosParaEliminar = await DeleteLogicalImpactos(request, evolucion, registroActualizacion.Id);
            await SaveEvolucionAsync(evolucion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Evolucion, ImpactoEvolucion, ManageImpactoDto>(
                registroActualizacion,
                evolucion,
                ApartadoRegistroEnum.ConsecuenciaActuacion,
                impactosParaEliminar, impactosOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateImpactoEvolucionCommandHandler)} - END");

            return new ManageImpactoResponse
            {
                IdEvolucion = evolucion.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageImpactosCommandHandler");
            throw;
        }
    }

    private async Task<Evolucion> GetOrCreateEvolucion(ManageImpactosCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsRegistro = new List<int>();
            List<int> idsDatoPrincipal = new List<int>();
            List<int> idsParametro = new List<int>();
            List<int> idsAreaAfectada = new List<int>();
            List<int> idsConsecuenciaActuacion = new List<int>();
            List<int> idsIntervencionMedio = new List<int>();

            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.ConsecuenciaActuacion)
                {
                    idsConsecuenciaActuacion.Add(detalle.IdReferencia);
                }
            }

            // Buscar la Evolucion por IdReferencia
            var evolucion = await _unitOfWork.Repository<Evolucion>()
                .GetByIdWithSpec(new EvolucionWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia,
                    idsRegistro,
                    idsDatoPrincipal,
                    idsParametro, 
                    idsAreaAfectada,
                    idsConsecuenciaActuacion, 
                    idsIntervencionMedio,
                    //includeRegistro: false,
                    //includeDatoPrincipal: false,
                    esFoto: false
                ));

            if (evolucion is null || evolucion.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de Evolucion");

            return evolucion;
        }

        // Validar si ya existe un registro de Evolucion para este suceso
        var spec = new EvolucionWithImpactosSpecification(new EvolucionSpecificationParams { IdSuceso = request.IdSuceso });
        var evolucionExistente = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(spec);

        return evolucionExistente ?? new Evolucion { IdSuceso = request.IdSuceso, EsFoto = false };
    }

    private async Task ValidateImpactosClasificadosAsync(ManageImpactosCommand request)
    {
        var idsImpactosClasificados = request.Impactos.Select(ie => ie.IdImpactoClasificado).Distinct();
        var impactosClasificadosExistentes = await _unitOfWork.Repository<ImpactoClasificado>().GetAsync(ic => idsImpactosClasificados.Contains(ic.Id));

        if (impactosClasificadosExistentes.Count() != idsImpactosClasificados.Count())
        {
            var idsImpactosClasificadosExistentes = impactosClasificadosExistentes.Select(ic => ic.Id).ToList();
            var idsImpactosClasificadosInvalidos = idsImpactosClasificados.Except(idsImpactosClasificadosExistentes).ToList();

            if (idsImpactosClasificadosInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de impacto clasificado: {string.Join(", ", idsImpactosClasificadosInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(ImpactoClasificado), string.Join(", ", idsImpactosClasificadosInvalidos));
            }
        }
    }

    private async Task ValidateTiposDanioAsync(ManageImpactosCommand request)
    {
        var idsTipoDanio = request.Impactos
            .Where(i => i.IdTipoDanio.HasValue)
            .Select(i => i.IdTipoDanio.Value)
            .Distinct()
            .ToList();

        if (idsTipoDanio.Any())
        {
            var tiposDanioExistentes = await _unitOfWork.Repository<TipoDanio>().GetAsync(t => idsTipoDanio.Contains(t.Id));
            if (tiposDanioExistentes.Count() != idsTipoDanio.Count())
            {
                var idsTipoDanioExistentes = tiposDanioExistentes.Select(t => t.Id).ToList();
                var idsTipoDanioInvalidos = idsTipoDanio.Except(idsTipoDanioExistentes).ToList();

                if (idsTipoDanioInvalidos.Any())
                {
                    _logger.LogWarning($"Los siguientes Id's de tipos de daño: {string.Join(", ", idsTipoDanioInvalidos)}, no se encontraron");
                    throw new NotFoundException(nameof(TipoDanio), string.Join(", ", idsTipoDanioInvalidos));
                }
            }
        }
    }

    private void MapAndManageImpactos(ManageImpactosCommand request, Evolucion evolucion)
    {
        foreach (var impactoDto in request.Impactos)
        {
            if (impactoDto.Id.HasValue && impactoDto.Id > 0)
            {
                var impactoExistente = evolucion.Impactos.FirstOrDefault(d => d.Id == impactoDto.Id.Value);
                if (impactoExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageImpactoDto>(impactoExistente);
                    var copiaNueva = _mapper.Map<ManageImpactoDto>(impactoDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(impactoDto, impactoExistente);
                        impactoExistente.Borrado = false;
                    }
                }
                else
                {
                    evolucion.Impactos.Add(_mapper.Map<ImpactoEvolucion>(impactoDto));
                }
            }
            else
            {
                evolucion.Impactos.Add(_mapper.Map<ImpactoEvolucion>(impactoDto));
            }
        }
    }

    private async Task<List<int>> DeleteLogicalImpactos(ManageImpactosCommand request, Evolucion evolucion, int idRegistroActualizacion)
    {
        if (evolucion.Id > 0)
        {
            var idsEnRequest = request.Impactos
                .Where(d => d.Id.HasValue && d.Id > 0)
                .Select(d => d.Id)
                .ToList();

            var impactosParaEliminar = evolucion.Impactos
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id) && d.Borrado == false)
                .ToList();

            if (impactosParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación
            var idsImpactosParaEliminar = impactosParaEliminar.Select(d => d.Id).ToList();
            var historialAreas = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsImpactosParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.ConsecuenciaActuacion);

            foreach (var impacto in impactosParaEliminar)
            {
                var historial = historialAreas.FirstOrDefault(d =>
                d.IdReferencia == impacto.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La consecuencia/impacto con ID {impacto.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<ImpactoEvolucion>().DeleteEntity(impacto);
            }
        }
        return new List<int>();
    }

    private async Task SaveEvolucionAsync(Evolucion evolucion)
    {
        if (evolucion.Id > 0)
        {
            _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);
        }
        else
        {
            _unitOfWork.Repository<Evolucion>().AddEntity(evolucion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la Evolucion");
    }
}
