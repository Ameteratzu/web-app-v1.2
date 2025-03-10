using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
internal class CreateOrUpdateDireccionCommandHandler : IRequestHandler<CreateOrUpdateDireccionCommand, CreateOrUpdateDireccionResponse>
{
    private readonly ILogger<CreateImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public CreateOrUpdateDireccionCommandHandler(
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

    public async Task<CreateOrUpdateDireccionResponse> Handle(CreateOrUpdateDireccionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateTipoDireccionEmergencia(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<DireccionCoordinacionEmergencia>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.DireccionCoordinacion);

            DireccionCoordinacionEmergencia direccionCoordinacionEmergencia = await GetOrCreateDireccionCoordinacion(request, registroActualizacion);

            var direccionesOriginales = direccionCoordinacionEmergencia.Direcciones.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateDireccionDto>(d));
            MapAndSaveDirecciones(request, direccionCoordinacionEmergencia);

            var direccionesParaEliminar = await DeleteLogicalDirecciones(request, direccionCoordinacionEmergencia, registroActualizacion.Id);

            await SaveDireccionCoordinacion(direccionCoordinacionEmergencia);

            await _registroActualizacionService.SaveRegistroActualizacion<
                DireccionCoordinacionEmergencia, Direccion, CreateOrUpdateDireccionDto>(
                registroActualizacion,
                direccionCoordinacionEmergencia,
                ApartadoRegistroEnum.Direccion,
                direccionesParaEliminar, direccionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - END");

            return new CreateOrUpdateDireccionResponse
            {
                IdDireccionCoordinacionEmergencia = direccionCoordinacionEmergencia.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateDireccionCommandHandler");
            throw;
        }
    }

    private async Task<DireccionCoordinacionEmergencia> GetOrCreateDireccionCoordinacion(CreateOrUpdateDireccionCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            List<int> idsDirecciones = new List<int>();
            List<int> idsCecopi = new List<int>();
            List<int> idsPma = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.Direccion:
                        idsDirecciones.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.CoordinacionCECOPI:
                        idsCecopi.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.CoordinacionPMA:
                        idsPma.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Dirección y Coordinación de Emergencia por IdReferencia
            var direccionCoordinacion = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>()
                .GetByIdWithSpec(new DireccionCoordinacionEmergenciaWithFilteredData(registroActualizacion.IdReferencia, idsDirecciones, idsCecopi, idsPma));

            if (direccionCoordinacion is null || direccionCoordinacion.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de Direccion y Coordinacion");

            return direccionCoordinacion;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new DireccionCoordinacionEmergenciaWithDirecciones(new DireccionCoordinacionEmergenciaParams { IdSuceso = request.IdSuceso });
        var direccionExistente = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetByIdWithSpec(specSuceso);

        return direccionExistente ?? new DireccionCoordinacionEmergencia { IdSuceso = request.IdSuceso };
    }

    private async Task ValidateTipoDireccionEmergencia(CreateOrUpdateDireccionCommand request)
    {
        var idsTipoDireccion = request.Direcciones.Select(d => d.IdTipoDireccionEmergencia).Distinct();
        var tiposDireccionExistentes = await _unitOfWork.Repository<TipoDireccionEmergencia>().GetAsync(td => idsTipoDireccion.Contains(td.Id));

        if (tiposDireccionExistentes.Count() != idsTipoDireccion.Count())
        {
            var idsInvalidos = idsTipoDireccion.Except(tiposDireccionExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(TipoDireccionEmergencia), string.Join(", ", idsInvalidos));
        }
    }

    private void MapAndSaveDirecciones(CreateOrUpdateDireccionCommand request, DireccionCoordinacionEmergencia direccionCoordinacion)
    {
        foreach (var direccionDto in request.Direcciones)
        {
            if (direccionDto.Id.HasValue && direccionDto.Id > 0)
            {
                var direccionExistente = direccionCoordinacion.Direcciones.FirstOrDefault(d => d.Id == direccionDto.Id.Value);
                if (direccionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateDireccionDto>(direccionExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateDireccionDto>(direccionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(direccionDto, direccionExistente);
                        direccionExistente.Borrado = false;
                    }
                }
                else
                {
                    Direccion newDireccion = _mapper.Map<Direccion>(direccionDto);
                    newDireccion.Id = 0;
                    direccionCoordinacion.Direcciones.Add(newDireccion);
                }
            }
            else
            {
                Direccion newDireccion = _mapper.Map<Direccion>(direccionDto);
                newDireccion.Id = 0;
                direccionCoordinacion.Direcciones.Add(newDireccion);
            }
        }
    }

    private async Task<List<int>> DeleteLogicalDirecciones(CreateOrUpdateDireccionCommand request, DireccionCoordinacionEmergencia direccionCoordinacion, int idRegistroActualizacion)
    {
        if (direccionCoordinacion.Id > 0)
        {
            var idsEnRequest = request.Direcciones.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var direccionesParaEliminar = direccionCoordinacion.Direcciones
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (direccionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsDireccionesParaEliminar = direccionesParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsDireccionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Direccion);

            foreach (var direccion in direccionesParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == direccion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La dirección con ID {direccion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<Direccion>().DeleteEntity(direccion);
            }

            return idsDireccionesParaEliminar;
        }

        return new List<int>();
    }

    private async Task SaveDireccionCoordinacion(DireccionCoordinacionEmergencia direccionCoordinacion)
    {
        if (direccionCoordinacion.Id > 0)
        {
            _unitOfWork.Repository<DireccionCoordinacionEmergencia>().UpdateEntity(direccionCoordinacion);
        }
        else
        {
            _unitOfWork.Repository<DireccionCoordinacionEmergencia>().AddEntity(direccionCoordinacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la Dirección y Coordinación de Emergencia");
    }
}
