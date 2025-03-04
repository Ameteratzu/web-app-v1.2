using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
using DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
using DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
public class ManageActivacionSistemaCommandHandler : IRequestHandler<ManageActivacionSistemaCommand, ManageActivacionSistemaResponse>
{

    private readonly ILogger<ManageActivacionSistemaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageActivacionSistemaCommandHandler(
        ILogger<ManageActivacionSistemaCommandHandler> logger,
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

    public async Task<ManageActivacionSistemaResponse> Handle(ManageActivacionSistemaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageNotificacionEmergenciaHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);

        await ValidateTipoSistemaEmergencia(request);
        await ValidateModosActivacion(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<ActuacionRelevanteDGPCE>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.ActuacionRelevante);

            ActuacionRelevanteDGPCE actuacion = await GetOrCreateActuacionRelevante(request, registroActualizacion);

            var activacionSistemasOriginales = actuacion.ActivacionSistemas.ToDictionary(d => d.Id, d => _mapper.Map<ManageActivacionSistemaDto>(d));
            MapAndSaveActivaciones(request, actuacion);

            var activacionesParaEliminar = await DeleteLogicalActivaciones(request, actuacion, registroActualizacion.Id);

            await SaveActuacion(actuacion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                ActuacionRelevanteDGPCE, ActivacionSistema, ManageActivacionSistemaDto>(
                registroActualizacion,
                actuacion,
                ApartadoRegistroEnum.ActivacionDeSistemas,
                activacionesParaEliminar, activacionSistemasOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageNotificacionEmergenciaHandler)} - END");

            return new ManageActivacionSistemaResponse
            {
                IdActuacionRelevante = actuacion.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageConvocatoriaCECODCommandHandler");
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


    private async Task<List<int>> DeleteLogicalActivaciones(ManageActivacionSistemaCommand request, ActuacionRelevanteDGPCE actuacion, int idRegistroActualizacion)
    {
        if (actuacion.Id > 0)
        {
            var idsEnRequest = request.Detalles.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var activacionesSistemasParaEliminar = actuacion.ActivacionSistemas
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (activacionesSistemasParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas convocatorias
            var idsactivacionesSistemasParaEliminar = activacionesSistemasParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsactivacionesSistemasParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.ActivacionDeSistemas);

            foreach (var declaracion in activacionesSistemasParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == declaracion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La Activacion Sistema con ID {declaracion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<ActivacionSistema>().DeleteEntity(declaracion);
            }

            return idsactivacionesSistemasParaEliminar;
        }

        return new List<int>();
    }


    private void MapAndSaveActivaciones(ManageActivacionSistemaCommand request, ActuacionRelevanteDGPCE actuacion)
    {
        foreach (var activacionDto in request.Detalles)
        {
            if (activacionDto.Id.HasValue && activacionDto.Id > 0)
            {
                var activacionExistente = actuacion.ActivacionSistemas.FirstOrDefault(d => d.Id == activacionDto.Id.Value);
                if (activacionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageActivacionSistemaDto>(activacionExistente);
                    var copiaNueva = _mapper.Map<ManageActivacionSistemaDto>(activacionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(activacionDto, activacionExistente);
                        activacionExistente.Borrado = false;
                    }
                }
                else
                {
                    actuacion.ActivacionSistemas.Add(_mapper.Map<ActivacionSistema>(activacionDto));
                }
            }
            else
            {
                actuacion.ActivacionSistemas.Add(_mapper.Map<ActivacionSistema>(activacionDto));
            }
        }
    }
    private async Task<ActuacionRelevanteDGPCE> GetOrCreateActuacionRelevante(ManageActivacionSistemaCommand request, RegistroActualizacion registroActualizacion)
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

            // Buscar la activacionSistema por IdReferencia
            var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
                .GetByIdWithSpec(new ActuacionRelevanteDGPCEWithFilteredData(registroActualizacion.IdReferencia, idsActivacionPlanEmergencias, idsDeclaracionesZAGEP, idsActivacionSistemas, idsConvocatoriasCECOD, idsNotificacionesEmergencias, idsMovilizacionMedios, includeEmergenciaNacional));

            if (actuacionRelevante is null || actuacionRelevante.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de actuacionRelevanteDGPCE");

            return actuacionRelevante;
        }

        // Validar si ya existe un registro de activacionSistema para este suceso
        var specSuceso = new ActuacionRelevanteDGPCEWithActivacionSistemas(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = request.IdSuceso });
        var activacionSistemaExistente = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(specSuceso);

        return activacionSistemaExistente ?? new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
    }


    private async Task ValidateModosActivacion(ManageActivacionSistemaCommand request)
    {
        var idsModosActivacion = request.Detalles.Select(c => c.IdModoActivacion).Where(c => c.HasValue).Distinct();
        var ModosActivacionExistentes = await _unitOfWork.Repository<ModoActivacion>().GetAsync(p => idsModosActivacion.Contains(p.Id));

        if (ModosActivacionExistentes.Count() != idsModosActivacion.Count())
        {
            var idsModosActivacionExistentes = ModosActivacionExistentes.Select(p => p.Id).Cast<int?>().ToList();
            var idsModosActivacionInvalidas = idsModosActivacion.Except(idsModosActivacionExistentes).ToList();

            if (idsModosActivacionInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Modos de activación: {string.Join(", ", idsModosActivacionInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(ModoActivacion), string.Join(", ", idsModosActivacionInvalidas));
            }
        }
    }


    private async Task ValidateTipoSistemaEmergencia(ManageActivacionSistemaCommand request)
    {
        var idsTipoSistemaEmergencia = request.Detalles.Select(c => c.IdTipoSistemaEmergencia).Distinct();
        var tipoSistemaEmergenciaExistentes = await _unitOfWork.Repository<TipoSistemaEmergencia>().GetAsync(p => idsTipoSistemaEmergencia.Contains(p.Id));

        if (tipoSistemaEmergenciaExistentes.Count() != idsTipoSistemaEmergencia.Count())
        {
            var idsTipoSistemaEmergenciasExistentes = tipoSistemaEmergenciaExistentes.Select(p => p.Id).Cast<int>().ToList();
            var idsTipoSistemaEmergenciasInvalidas = idsTipoSistemaEmergencia.Except(idsTipoSistemaEmergenciasExistentes).ToList();

            if (idsTipoSistemaEmergenciasInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Sistema Emergencia: {string.Join(", ", idsTipoSistemaEmergenciasInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(TipoPlan), string.Join(", ", idsTipoSistemaEmergenciasInvalidas));
            }
        }
    }


}