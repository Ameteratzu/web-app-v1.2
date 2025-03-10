using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
public class ManageActivacionPlanEmergenciaCommandHandler : IRequestHandler<ManageActivacionPlanEmergenciaCommand, ManageActivacionPlanEmergenciaResponse>
{
    private readonly ILogger<ManageActivacionPlanEmergenciaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private const string ARCHIVOS_PATH = "activacion-plan-emergencia";
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageActivacionPlanEmergenciaCommandHandler(
        ILogger<ManageActivacionPlanEmergenciaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService,
        IRegistroActualizacionService registroActualizacionService
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageActivacionPlanEmergenciaResponse> Handle(ManageActivacionPlanEmergenciaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageActivacionPlanEmergenciaCommandHandler)} - BEGIN");

        await ValidateTipoPlanes(request);
        await ValidatePlanesEmergencias(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<ActuacionRelevanteDGPCE>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.ActuacionRelevante);

            ActuacionRelevanteDGPCE actuacion = await GetOrCreateActuacionRelevante(request, registroActualizacion);

            var activacionPlanesOriginales = actuacion.ActivacionPlanEmergencias.ToDictionary(d => d.Id, d => _mapper.Map<ManageActivacionPlanEmergenciaDto>(d));
            MapAndSaveDetallesActivacionPlanEmergencia(request, actuacion);

            var activacionPlanesParaEliminar = await DeleteLogicalActivacionesPlanes(request, actuacion, registroActualizacion.Id);

            await SaveActuacion(actuacion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                ActuacionRelevanteDGPCE, ActivacionPlanEmergencia, ManageActivacionPlanEmergenciaDto>(
                registroActualizacion,
                actuacion,
                ApartadoRegistroEnum.ActivacionDePlanes,
                activacionPlanesParaEliminar, activacionPlanesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageActivacionPlanEmergenciaCommandHandler)} - END");

            return new ManageActivacionPlanEmergenciaResponse
            {
                IdActuacionRelevante = actuacion.Id,
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

    private async Task SaveActuacion(ActuacionRelevanteDGPCE actuacion)
    {
        if (actuacion.Id > 0)
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacion);
        }
        else
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().AddEntity(actuacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la ActuacionRelevanteDGPCE");
    }


    private async Task<List<int>> DeleteLogicalActivacionesPlanes(ManageActivacionPlanEmergenciaCommand request, ActuacionRelevanteDGPCE actuacion, int idRegistroActualizacion)
    {
        if (actuacion.Id > 0)
        {
            var idsEnRequest = request.ActivacionesPlanes.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var activacionDePlanesParaEliminar = actuacion.ActivacionPlanEmergencias
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (activacionDePlanesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas activacionDePlanes
            var idsActivacionDePlanesParaEliminarParaEliminar = activacionDePlanesParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsActivacionDePlanesParaEliminarParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.ActivacionDePlanes);

            foreach (var detalle in activacionDePlanesParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == detalle.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"Las activacion de plan de emergencia con ID {detalle.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                if (detalle.IdArchivo.HasValue)
                {
                    _unitOfWork.Repository<Archivo>().DeleteEntity(detalle.Archivo);
                }

                _unitOfWork.Repository<ActivacionPlanEmergencia>().DeleteEntity(detalle);
            }

            return idsActivacionDePlanesParaEliminarParaEliminar;
        }

        return new List<int>();
    }


    private async void MapAndSaveDetallesActivacionPlanEmergencia(ManageActivacionPlanEmergenciaCommand request, ActuacionRelevanteDGPCE actuacion)
    {
        foreach (var detalleActivacionPlanesDto in request.ActivacionesPlanes)
        {
            if (detalleActivacionPlanesDto.Id.HasValue && detalleActivacionPlanesDto.Id > 0)
            {
                var activacionPlanesExistente = actuacion.ActivacionPlanEmergencias.FirstOrDefault(d => d.Id == detalleActivacionPlanesDto.Id.Value);
                if (activacionPlanesExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageActivacionPlanEmergenciaDto>(activacionPlanesExistente);
                    var copiaNueva = _mapper.Map<ManageActivacionPlanEmergenciaDto>(detalleActivacionPlanesDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(detalleActivacionPlanesDto, activacionPlanesExistente);
                        activacionPlanesExistente.Borrado = false;
                        activacionPlanesExistente.Archivo = await MapArchivo(detalleActivacionPlanesDto, activacionPlanesExistente.Archivo);
                    }
                }
                else
                {
                    actuacion.ActivacionPlanEmergencias.Add(await CreateDetalleActivacionPlanesEmergencia(detalleActivacionPlanesDto));
                }
            }
            else
            {
                actuacion.ActivacionPlanEmergencias.Add(await CreateDetalleActivacionPlanesEmergencia(detalleActivacionPlanesDto));
            }
        }
    }

    private async Task<ActivacionPlanEmergencia> CreateDetalleActivacionPlanesEmergencia (ManageActivacionPlanEmergenciaDto detalleActivacionPlanesDto)
    {
        var nuevoDetalleActivacionPlanEmergencia = new ActivacionPlanEmergencia
        {
            IdTipoPlan = detalleActivacionPlanesDto.IdTipoPlan,
            TipoPlanPersonalizado = detalleActivacionPlanesDto.TipoPlanPersonalizado,
            IdPlanEmergencia = detalleActivacionPlanesDto.IdPlanEmergencia,
            PlanEmergenciaPersonalizado = detalleActivacionPlanesDto.PlanEmergenciaPersonalizado,
            FechaInicio = detalleActivacionPlanesDto.FechaInicio,
            FechaFin = detalleActivacionPlanesDto.FechaFin,
            Autoridad = detalleActivacionPlanesDto.Autoridad,
            Observaciones = detalleActivacionPlanesDto.Observaciones
        };

        nuevoDetalleActivacionPlanEmergencia.Archivo = await MapArchivo(detalleActivacionPlanesDto, null);

        return nuevoDetalleActivacionPlanEmergencia;
    }


    private async Task<Archivo?> MapArchivo(ManageActivacionPlanEmergenciaDto activacionPlanesEmergenciaDto, Archivo? archivoExistente)
    {
        if (activacionPlanesEmergenciaDto.Archivo != null)
        {
            var fileEntity = new Archivo
            {
                NombreOriginal = activacionPlanesEmergenciaDto.Archivo?.FileName ?? string.Empty,
                NombreUnico = $"{Path.GetFileNameWithoutExtension(activacionPlanesEmergenciaDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{activacionPlanesEmergenciaDto.Archivo?.Extension ?? string.Empty}",
                Tipo = activacionPlanesEmergenciaDto.Archivo?.ContentType ?? string.Empty,
                Extension = activacionPlanesEmergenciaDto.Archivo?.Extension ?? string.Empty,
                PesoEnBytes = activacionPlanesEmergenciaDto.Archivo?.Length ?? 0,
            };

            fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(activacionPlanesEmergenciaDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
            fileEntity.FechaCreacion = DateTime.Now;
            return fileEntity;
        }

        return archivoExistente;
    }



    private async Task<ActuacionRelevanteDGPCE> GetOrCreateActuacionRelevante(ManageActivacionPlanEmergenciaCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            bool includeEmergenciaNacional = false;
            List<int> idsActivacionPlanEmergencias = new List<int>();
            List<int> idsDeclaracionesZAGEP = new List<int>();
            List<int> idsActivacionSistemas = new List<int>();
            List<int> idsConvocatoriasCECOD = new List<int>();
            List<int> idsNotificacionesEmergencias = new List<int>();
            List<int> idsMovilizacionMedios = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.EmergenciaNacional:
                        includeEmergenciaNacional = true;
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDePlanes:
                        idsActivacionPlanEmergencias.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.DeclaracionZAGEP:
                        idsDeclaracionesZAGEP.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDeSistemas:
                        idsActivacionSistemas.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.ConvocatoriaCECOD:
                        idsConvocatoriasCECOD.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.NotificacionesOficiales:
                        idsNotificacionesEmergencias.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.MovilizacionMedios:
                        idsMovilizacionMedios.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la activacion de planes de Emergencia por IdReferencia
            var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
                .GetByIdWithSpec(new ActuacionRelevanteDGPCEWithFilteredData(registroActualizacion.IdReferencia, idsActivacionPlanEmergencias, idsDeclaracionesZAGEP, idsActivacionSistemas, idsConvocatoriasCECOD, idsNotificacionesEmergencias, idsMovilizacionMedios, includeEmergenciaNacional));

            if (actuacionRelevante is null || actuacionRelevante.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de actuacionRelevanteDGPCE");

            return actuacionRelevante;
        }

        // Validar si ya existe un registro de activacion de planes de Emergencia para este suceso
        var specSuceso = new ActuacionRelevanteDGPCEWithActivacionPlanEmergencia(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = request.IdSuceso });
        var declaracionZAGEPExistente = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(specSuceso);

        return declaracionZAGEPExistente ?? new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
    }



    private async Task ValidateTipoPlanes(ManageActivacionPlanEmergenciaCommand request)
    {
        var idsTipoPlan = request.ActivacionesPlanes.Select(c => c.IdTipoPlan).Distinct();
        var tipoPlanesExistentes = await _unitOfWork.Repository<TipoPlan>().GetAsync(p => idsTipoPlan.Contains(p.Id));

        if (tipoPlanesExistentes.Count() != idsTipoPlan.Count())
        {
            var idsTipoPlanesExistentes = tipoPlanesExistentes.Select(p => p.Id).Cast<int?>().ToList();
            var idsTipoPlanesInvalidas = idsTipoPlan.Except(idsTipoPlanesExistentes).ToList();

            if (idsTipoPlanesInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Tipo Plan: {string.Join(", ", idsTipoPlanesInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(TipoPlan), string.Join(", ", idsTipoPlanesInvalidas));
            }
        }
    }

    private async Task ValidatePlanesEmergencias(ManageActivacionPlanEmergenciaCommand request)
    {
        var idsPlanesEmergencias = request.ActivacionesPlanes.Select(c => c.IdPlanEmergencia).Distinct();
        var planesEmergenciasExistentes = await _unitOfWork.Repository<PlanEmergencia>().GetAsync(p => idsPlanesEmergencias.Contains(p.Id));

        if (planesEmergenciasExistentes.Count() != idsPlanesEmergencias.Count())
        {
            var idsPlanesEmergenciasExistentes = planesEmergenciasExistentes.Select(p => p.Id).Cast<int?>().ToList();
            var idsPlanesEmergenciasInvalidas = idsPlanesEmergencias.Except(idsPlanesEmergenciasExistentes).ToList();

            if (idsPlanesEmergenciasInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Planes Emergencias: {string.Join(", ", idsPlanesEmergenciasInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(PlanEmergencia), string.Join(", ", idsPlanesEmergenciasInvalidas));
            }
        }
    }


}
