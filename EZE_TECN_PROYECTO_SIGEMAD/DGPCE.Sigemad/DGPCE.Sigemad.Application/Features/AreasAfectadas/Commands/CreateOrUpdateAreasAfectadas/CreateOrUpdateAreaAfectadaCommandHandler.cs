using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateOrUpdateAreasAfectadas;
public class CreateOrUpdateAreaAfectadaCommandHandler : IRequestHandler<CreateOrUpdateAreaAfectadaCommand, CreateOrUpdateAreaAfectadaResponse>
{
    private readonly ILogger<CreateIncendioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeometryValidator _geometryValidator;
    private readonly ICoordinateTransformationService _coordinateTransformationService;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public CreateOrUpdateAreaAfectadaCommandHandler(
        ILogger<CreateIncendioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IGeometryValidator geometryValidator,
        ICoordinateTransformationService coordinateTransformationService,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _geometryValidator = geometryValidator;
        _coordinateTransformationService = coordinateTransformationService;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }


    public async Task<CreateOrUpdateAreaAfectadaResponse> Handle(CreateOrUpdateAreaAfectadaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOrUpdateAreaAfectadaCommandHandler) + " - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateProvincia(request);
        await ValidateMunicipio(request);
        await ValidateEntidadMenor(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Evolucion>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Evolucion);

            var evolucion = await GetOrCreateEvolucion(request, registroActualizacion);

            var coordinacionesOriginales = evolucion.AreaAfectadas.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateAreaAfectadaDto>(d));
            MapAndSaveAreasAfectadas(request, evolucion);

            var areasParaEliminar = await DeleteLogicalAreasAfectadas(request, evolucion, registroActualizacion.Id);
            await SaveEvolucion(evolucion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Evolucion, AreaAfectada, CreateOrUpdateAreaAfectadaDto>(
                registroActualizacion,
                evolucion,
                ApartadoRegistroEnum.AreaAfectada,
                areasParaEliminar, coordinacionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateAreaAfectadaCommandHandler)} - END");
            return new CreateOrUpdateAreaAfectadaResponse
            {
                IdEvolucion = evolucion.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateAreaAfectadaCommandHandler");
            throw;
        }
    }

    private async Task<Evolucion> GetOrCreateEvolucion(CreateOrUpdateAreaAfectadaCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsAreaAfectada = new List<int>();
            List<int> idsConsecuenciaActuacion = new List<int>();
            List<int> idsIntervencionMedio = new List<int>();

            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.AreaAfectada)
                {
                    idsAreaAfectada.Add(detalle.IdReferencia);
                }
            }

            // Buscar la Evolucion por IdReferencia
            var evolucion = await _unitOfWork.Repository<Evolucion>()
                .GetByIdWithSpec(new EvolucionWithFilteredDataSpecification(registroActualizacion.IdReferencia, idsAreaAfectada, 
                idsConsecuenciaActuacion, idsIntervencionMedio,
                esFoto: false,
                includeRegistroParametro: false));

            if (evolucion is null || evolucion.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de Evolucion");

            return evolucion;
        }

        // Validar si ya existe un registro de Evolucion para este suceso
        var spec = new EvolucionWithAreaAfectadaSpecification(new EvolucionSpecificationParams { IdSuceso = request.IdSuceso });
        var evolucionExistente = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(spec);

        return evolucionExistente ?? new Evolucion { IdSuceso = request.IdSuceso, EsFoto = false };
    }

    private async Task ValidateProvincia(CreateOrUpdateAreaAfectadaCommand request)
    {
        var idsProvincia = request.AreasAfectadas.Select(d => d.IdProvincia).Distinct();
        var provinciasExistentes = await _unitOfWork.Repository<Provincia>().GetAsync(td => idsProvincia.Contains(td.Id) && td.Borrado == false);

        if (provinciasExistentes.Count() != idsProvincia.Count())
        {
            var idsInvalidos = idsProvincia.Except(provinciasExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Provincia), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateMunicipio(CreateOrUpdateAreaAfectadaCommand request)
    {
        var idsMunicipio = request.AreasAfectadas.Select(d => d.IdMunicipio).Distinct();
        var municipiosExistentes = await _unitOfWork.Repository<Municipio>().GetAsync(td => idsMunicipio.Contains(td.Id) && td.Borrado == false);

        if (municipiosExistentes.Count() != idsMunicipio.Count())
        {
            var idsInvalidos = idsMunicipio.Except(municipiosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Municipio), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateEntidadMenor(CreateOrUpdateAreaAfectadaCommand request)
    {
        var idsEntidadMenor = request.AreasAfectadas.Where(a => a.IdEntidadMenor.HasValue).Select(d => d.IdMunicipio).Distinct();

        if (!idsEntidadMenor.Any()) return;

        var entidadExistentes = await _unitOfWork.Repository<EntidadMenor>().GetAsync(td => idsEntidadMenor.Contains(td.Id) && td.Borrado == false);

        if (entidadExistentes.Count() != idsEntidadMenor.Count())
        {
            var idsInvalidos = idsEntidadMenor.Except(entidadExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Municipio), string.Join(", ", idsInvalidos));
        }
    }

    private void MapAndSaveAreasAfectadas(CreateOrUpdateAreaAfectadaCommand request, Evolucion evolucion)
    {
        foreach (var areaAfectadaDto in request.AreasAfectadas)
        {
            if (areaAfectadaDto.Id.HasValue && areaAfectadaDto.Id > 0)
            {
                var areaExistente = evolucion.AreaAfectadas.FirstOrDefault(d => d.Id == areaAfectadaDto.Id.Value);
                if (areaExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateAreaAfectadaDto>(areaExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateAreaAfectadaDto>(areaAfectadaDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(areaAfectadaDto, areaExistente);
                        areaExistente.Borrado = false;
                    }
                }
                else
                {
                    evolucion.AreaAfectadas.Add(_mapper.Map<AreaAfectada>(areaAfectadaDto));
                }
            }
            else
            {
                evolucion.AreaAfectadas.Add(_mapper.Map<AreaAfectada>(areaAfectadaDto));
            }
        }
    }

    private async Task<List<int>> DeleteLogicalAreasAfectadas(CreateOrUpdateAreaAfectadaCommand request, Evolucion evolucion, int idRegistroActualizacion)
    {
        if (evolucion.Id > 0)
        {
            var idsEnRequest = request.AreasAfectadas.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var areasParaEliminar = evolucion.AreaAfectadas
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id) && d.Borrado == false)
                .ToList();

            if (areasParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsAreasParaEliminar = areasParaEliminar.Select(d => d.Id).ToList();
            var historialAreas = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsAreasParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.AreaAfectada);

            foreach (var area in areasParaEliminar)
            {
                var historial = historialAreas.FirstOrDefault(d =>
                d.IdReferencia == area.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"El area afectada con ID {area.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<AreaAfectada>().DeleteEntity(area);
            }

            return idsAreasParaEliminar;
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
