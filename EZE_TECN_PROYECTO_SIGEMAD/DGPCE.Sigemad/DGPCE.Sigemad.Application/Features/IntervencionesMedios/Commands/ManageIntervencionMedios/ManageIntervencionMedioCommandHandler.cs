using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.ManageIntervencionMedios;
public class ManageIntervencionMedioCommandHandler : IRequestHandler<ManageIntervencionMedioCommand, ManageIntervencionMedioResponse>
{
    private readonly ILogger<ManageIntervencionMedioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;
    public ManageIntervencionMedioCommandHandler(
    ILogger<ManageIntervencionMedioCommandHandler> logger,
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

    public async Task<ManageIntervencionMedioResponse> Handle(ManageIntervencionMedioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateCaracterMedio(request);
        await ValidateTitularMedio(request);
        await ValidateProvincia(request);
        await ValidateMunicipio(request);
        await ValidateMediosCapacidad(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Evolucion>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Evolucion);

            var evolucion = await GetOrCreateEvolucion(request, registroActualizacion);

            var intervencionesOriginales = evolucion.IntervencionMedios.ToDictionary(d => d.Id, d => _mapper.Map<ManageIntervencionMedioDto>(d));
            MapAndSaveIntervenciones(request, evolucion);

            var intervencionesParaEliminar = await DeleteLogicalIntervenciones(request, evolucion, registroActualizacion.Id);
            await SaveEvolucion(evolucion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Evolucion, IntervencionMedio, ManageIntervencionMedioDto>(
                registroActualizacion,
                evolucion,
                ApartadoRegistroEnum.IntervencionMedios,
                intervencionesParaEliminar, intervencionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - END");
            return new ManageIntervencionMedioResponse
            {
                IdEvolucion = evolucion.Id,
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

    private async Task<Evolucion> GetOrCreateEvolucion(ManageIntervencionMedioCommand request, RegistroActualizacion registroActualizacion)
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
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.IntervencionMedios)
                {
                    idsIntervencionMedio.Add(detalle.IdReferencia);
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
        var spec = new EvolucionWithIntervencionSpecification(new EvolucionSpecificationParams { IdSuceso = request.IdSuceso });
        var evolucionExistente = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(spec);

        return evolucionExistente ?? new Evolucion { IdSuceso = request.IdSuceso, EsFoto = false };
    }

    private async Task ValidateCaracterMedio(ManageIntervencionMedioCommand request)
    {
        var idsCaracterMedio = request.Intervenciones.Select(d => d.IdCaracterMedio).Distinct();
        var caracterMediosExistentes = await _unitOfWork.Repository<CaracterMedio>().GetAsync(td => idsCaracterMedio.Contains(td.Id) && td.Obsoleto == false);

        if (caracterMediosExistentes.Count() != idsCaracterMedio.Count())
        {
            var idsInvalidos = idsCaracterMedio.Except(caracterMediosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(CaracterMedio), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateTitularMedio(ManageIntervencionMedioCommand request)
    {
        var idsTitularMedio = request.Intervenciones.Select(d => d.IdCaracterMedio).Distinct();
        var titularMediosExistentes = await _unitOfWork.Repository<TitularidadMedio>().GetAsync(td => idsTitularMedio.Contains(td.Id));

        if (titularMediosExistentes.Count() != idsTitularMedio.Count())
        {
            var idsInvalidos = idsTitularMedio.Except(titularMediosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(TitularidadMedio), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateProvincia(ManageIntervencionMedioCommand request)
    {
        var idsProvincia = request.Intervenciones.Select(d => d.IdProvincia).Distinct();
        var provinciasExistentes = await _unitOfWork.Repository<Provincia>().GetAsync(td => idsProvincia.Contains(td.Id) && td.Borrado == false);

        if (provinciasExistentes.Count() != idsProvincia.Count())
        {
            var idsInvalidos = idsProvincia.Except(provinciasExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Provincia), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateMunicipio(ManageIntervencionMedioCommand request)
    {
        var idsMunicipio = request.Intervenciones.Select(d => d.IdMunicipio).Distinct();
        var municipiosExistentes = await _unitOfWork.Repository<Municipio>().GetAsync(td => idsMunicipio.Contains(td.Id) && td.Borrado == false);

        if (municipiosExistentes.Count() != idsMunicipio.Count())
        {
            var idsInvalidos = idsMunicipio.Except(municipiosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Municipio), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateMediosCapacidad(ManageIntervencionMedioCommand request)
    {
        // Flatten the nested lists into a single list of unique IDs
        var idsMediosCapacidad = request.Intervenciones
            .SelectMany(d => d.DetalleIntervencionMedios.Select(dt => dt.IdMediosCapacidad))
            .Distinct()
            .ToList(); // Ensure it is a list for Contains() operation

        if (!idsMediosCapacidad.Any())
        {
            return; // No capacity IDs to validate
        }
        var mediosCapacidadExistentes = await _unitOfWork.Repository<MediosCapacidad>().GetAsync(td => idsMediosCapacidad.Contains(td.Id));

        if (mediosCapacidadExistentes.Count() != idsMediosCapacidad.Count())
        {
            var idsInvalidos = idsMediosCapacidad.Except(mediosCapacidadExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(MediosCapacidad), string.Join(", ", idsInvalidos));
        }
    }

    private void MapAndSaveIntervenciones(ManageIntervencionMedioCommand request, Evolucion evolucion)
    {
        foreach (var intervencionDto in request.Intervenciones)
        {
            if (intervencionDto.Id.HasValue && intervencionDto.Id > 0)
            {
                var intervencionExistente = evolucion.IntervencionMedios.FirstOrDefault(d => d.Id == intervencionDto.Id.Value);
                if (intervencionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageIntervencionMedioDto>(intervencionExistente);
                    var copiaNueva = _mapper.Map<ManageIntervencionMedioDto>(intervencionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(intervencionDto, intervencionExistente);
                        intervencionExistente.Borrado = false;
                    }
                }
                else
                {
                    evolucion.IntervencionMedios.Add(_mapper.Map<IntervencionMedio>(intervencionDto));
                }
            }
            else
            {
                evolucion.IntervencionMedios.Add(_mapper.Map<IntervencionMedio>(intervencionDto));
            }
        }
    }

    private async Task<List<int>> DeleteLogicalIntervenciones(ManageIntervencionMedioCommand request, Evolucion evolucion, int idRegistroActualizacion)
    {
        if (evolucion.Id > 0)
        {
            var idsEnRequest = request.Intervenciones.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var intervencionesParaEliminar = evolucion.IntervencionMedios
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id) && d.Borrado == false)
                .ToList();

            if (intervencionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsIntervencionesParaEliminar = intervencionesParaEliminar.Select(d => d.Id).ToList();
            var historialIntervenciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsIntervencionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.AreaAfectada);

            foreach (var intervencion in intervencionesParaEliminar)
            {
                var historial = historialIntervenciones.FirstOrDefault(d =>
                d.IdReferencia == intervencion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"Intervencion de medio con ID {intervencion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<IntervencionMedio>().DeleteEntity(intervencion);
            }

            return idsIntervencionesParaEliminar;
        }

        return new List<int>();
    }

    private async Task SaveEvolucion(Evolucion evolucion)
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
