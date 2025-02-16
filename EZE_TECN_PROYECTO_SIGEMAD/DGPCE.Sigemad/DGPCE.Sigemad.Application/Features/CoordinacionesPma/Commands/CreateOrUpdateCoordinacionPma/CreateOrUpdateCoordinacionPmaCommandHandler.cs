using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.CoordinacionesPma.Commands.CreateOrUpdateCoordinacionPma;
public class CreateOrUpdateCoordinacionPmaCommandHandler : IRequestHandler<CreateOrUpdateCoordinacionPmaCommand, CreateOrUpdateCoordinacionPmaResponse>
{
    private readonly ILogger<CreateOrUpdateCoordinacionPmaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public CreateOrUpdateCoordinacionPmaCommandHandler(
        ILogger<CreateOrUpdateCoordinacionPmaCommandHandler> logger,
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

    public async Task<CreateOrUpdateCoordinacionPmaResponse> Handle(CreateOrUpdateCoordinacionPmaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateCoordinacionPmaCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateProvincia(request);
        await ValidateMunicipio(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<DireccionCoordinacionEmergencia>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.DireccionCoordinacion);

            DireccionCoordinacionEmergencia direccionCoordinacionEmergencia = await GetOrCreateDireccionCoordinacion(request, registroActualizacion);

            var coordinacionesOriginales = direccionCoordinacionEmergencia.CoordinacionesCecopi.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateCoordinacionPmaDto>(d));
            MapAndSaveCoordinaciones(request, direccionCoordinacionEmergencia);

            var coordinacionesParaEliminar = await DeleteLogicalCoordinaciones(request, direccionCoordinacionEmergencia, registroActualizacion.Id);
            await SaveDireccionCoordinacion(direccionCoordinacionEmergencia);

            await _registroActualizacionService.SaveRegistroActualizacion<
                DireccionCoordinacionEmergencia, CoordinacionPMA, CreateOrUpdateCoordinacionPmaDto>(
                registroActualizacion,
                direccionCoordinacionEmergencia,
                ApartadoRegistroEnum.CoordinacionPMA,
                coordinacionesParaEliminar, coordinacionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateCoordinacionPmaCommandHandler)} - END");
            return new CreateOrUpdateCoordinacionPmaResponse
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


    private async Task<DireccionCoordinacionEmergencia> GetOrCreateDireccionCoordinacion(CreateOrUpdateCoordinacionPmaCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsDirecciones = new List<int>();
            List<int> idsCecopi = new List<int>();
            List<int> idsPma = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.CoordinacionPMA)
                {
                    idsCecopi.Add(detalle.IdReferencia);
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
        var specSuceso = new DireccionCoordinacionEmergenciaWithCoordinacionPma(new DireccionCoordinacionEmergenciaParams { IdSuceso = request.IdSuceso });
        var direccionExistente = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetByIdWithSpec(specSuceso);

        return direccionExistente ?? new DireccionCoordinacionEmergencia { IdSuceso = request.IdSuceso };
    }

    private async Task ValidateProvincia(CreateOrUpdateCoordinacionPmaCommand request)
    {
        var idsProvincia = request.Coordinaciones.Select(d => d.IdProvincia).Distinct();
        var provinciasExistentes = await _unitOfWork.Repository<Provincia>().GetAsync(td => idsProvincia.Contains(td.Id));

        if (provinciasExistentes.Count() != idsProvincia.Count())
        {
            var idsInvalidos = idsProvincia.Except(provinciasExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Provincia), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateMunicipio(CreateOrUpdateCoordinacionPmaCommand request)
    {
        var idsMunicipio = request.Coordinaciones.Select(d => d.IdMunicipio).Distinct();
        var municipiosExistentes = await _unitOfWork.Repository<Municipio>().GetAsync(td => idsMunicipio.Contains(td.Id));

        if (municipiosExistentes.Count() != idsMunicipio.Count())
        {
            var idsInvalidos = idsMunicipio.Except(municipiosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Municipio), string.Join(", ", idsInvalidos));
        }
    }

    private void MapAndSaveCoordinaciones(CreateOrUpdateCoordinacionPmaCommand request, DireccionCoordinacionEmergencia direccionCoordinacion)
    {
        foreach (var coordinacionDto in request.Coordinaciones)
        {
            if (coordinacionDto.Id.HasValue && coordinacionDto.Id > 0)
            {
                var coordinacionExistente = direccionCoordinacion.CoordinacionesPMA.FirstOrDefault(d => d.Id == coordinacionDto.Id.Value);
                if (coordinacionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateCoordinacionPmaDto>(coordinacionExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateCoordinacionPmaDto>(coordinacionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(coordinacionDto, coordinacionExistente);
                        coordinacionExistente.Borrado = false;
                    }
                }
                else
                {
                    direccionCoordinacion.CoordinacionesPMA.Add(_mapper.Map<CoordinacionPMA>(coordinacionDto));
                }
            }
            else
            {
                direccionCoordinacion.CoordinacionesPMA.Add(_mapper.Map<CoordinacionPMA>(coordinacionDto));
            }
        }
    }

    private async Task<List<int>> DeleteLogicalCoordinaciones(CreateOrUpdateCoordinacionPmaCommand request, DireccionCoordinacionEmergencia direccionCoordinacion, int idRegistroActualizacion)
    {
        if (direccionCoordinacion.Id > 0)
        {
            var idsEnRequest = request.Coordinaciones.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var coordinacionesParaEliminar = direccionCoordinacion.CoordinacionesPMA
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (coordinacionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsCoordinacionesParaEliminar = coordinacionesParaEliminar.Select(d => d.Id).ToList();
            var historialCoordinaciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsCoordinacionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.CoordinacionPMA);

            foreach (var coordinacion in coordinacionesParaEliminar)
            {
                var historial = historialCoordinaciones.FirstOrDefault(d =>
                d.IdReferencia == coordinacion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La coordinacion PMA con ID {coordinacion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<CoordinacionPMA>().DeleteEntity(coordinacion);
            }

            return idsCoordinacionesParaEliminar;
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
