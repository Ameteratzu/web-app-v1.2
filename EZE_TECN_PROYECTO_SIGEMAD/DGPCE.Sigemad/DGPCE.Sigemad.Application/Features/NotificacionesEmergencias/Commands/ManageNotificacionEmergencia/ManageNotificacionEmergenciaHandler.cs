using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
public class ManageNotificacionEmergenciaHandler : IRequestHandler<ManageNotificacionEmergenciaCommand, ManageNotificacionEmergenciaResponse>
{
    private readonly ILogger<ManageNotificacionEmergenciaHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageNotificacionEmergenciaHandler(
        ILogger<ManageNotificacionEmergenciaHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageNotificacionEmergenciaResponse> Handle(ManageNotificacionEmergenciaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageNotificacionEmergenciaHandler)} - BEGIN");

        var actuacionRelevante = await GetOrCreateActuacionRelevante(request);

        await ValidateTipoNotificacion(request);

        await MapAndSaveNotificacionEmergencia(request, actuacionRelevante);

        await DeleteLogicalNotificacionEmergencia(request, actuacionRelevante);
        await SaveActuacionRelevante(request, actuacionRelevante);

        _logger.LogInformation($"{nameof(ManageNotificacionEmergenciaHandler)} - END");
        return new ManageNotificacionEmergenciaResponse { IdActuacionRelevante = actuacionRelevante.Id };
    }

    private async Task DeleteLogicalNotificacionEmergencia(ManageNotificacionEmergenciaCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante > 0)
        {
            var idsEnRequest = request.Detalles
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var itemsParaEliminar = actuacionRelevante.NotificacionEmergencia
                .Where(c => c.Id > 0 && c.Borrado == false && !idsEnRequest.Contains(c.Id))
                .ToList();

            foreach (var detalleAEliminar in itemsParaEliminar)
            {
                _unitOfWork.Repository<NotificacionEmergencia>().DeleteEntity(detalleAEliminar);
            }
        }
    }


    private async Task SaveActuacionRelevante(ManageNotificacionEmergenciaCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante.Value > 0)
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacionRelevante);
        }
        else
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().AddEntity(actuacionRelevante);
        }

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar los detalles de la notificación emergencia ");
        }
    }


    private async Task MapAndSaveNotificacionEmergencia(ManageNotificacionEmergenciaCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        foreach (var notificacionEmergenciaDto in request.Detalles)
        {
            bool crearNueva = true;

            if (notificacionEmergenciaDto.Id.HasValue && notificacionEmergenciaDto.Id > 0)
            {
                var notificacionEmergencia = actuacionRelevante.NotificacionEmergencia!.FirstOrDefault(a => a.Id == notificacionEmergenciaDto.Id.Value);
                if (notificacionEmergencia != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(notificacionEmergenciaDto, notificacionEmergencia);
                    notificacionEmergencia.Borrado = false;
                    crearNueva = false;
                }
            }

            if (crearNueva)
            {
                // Crear nueva notificación emergencia
                var nuevaNotificacionEmergencia = _mapper.Map<NotificacionEmergencia>(notificacionEmergenciaDto);
                nuevaNotificacionEmergencia.Id = 0;

                actuacionRelevante.NotificacionEmergencia = actuacionRelevante.NotificacionEmergencia != null ? actuacionRelevante.NotificacionEmergencia : new List<NotificacionEmergencia>();
                actuacionRelevante.NotificacionEmergencia.Add(nuevaNotificacionEmergencia);
            }
        }
    }



    private async Task ValidateTipoNotificacion(ManageNotificacionEmergenciaCommand request)
    {
        var idsTipoNotificacion = request.Detalles.Select(c => c.IdTipoNotificacion).Distinct();
        var tipoNotificacionExistentes = await _unitOfWork.Repository<TipoNotificacion>().GetAsync(p => idsTipoNotificacion.Contains(p.Id));

        if (tipoNotificacionExistentes.Count() != idsTipoNotificacion.Count())
        {
            var idsTipoNotificacionesExistentes = tipoNotificacionExistentes.Select(p => p.Id).Cast<int>().ToList();
            var idsTipoNotificacionesInvalidas = idsTipoNotificacion.Except(idsTipoNotificacionesExistentes).ToList();

            if (idsTipoNotificacionesInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de tipo notificacion: {string.Join(", ", idsTipoNotificacionesInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(TipoPlan), string.Join(", ", idsTipoNotificacionesInvalidas));
            }
        }
    }

    private async Task<ActuacionRelevanteDGPCE> GetOrCreateActuacionRelevante(ManageNotificacionEmergenciaCommand request)
    {
        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante.Value > 0)
        {
            var spec = new ActuacionRelevanteDGPCESpecification(request.IdActuacionRelevante.Value);
            var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(spec);
            if (actuacionRelevante is null || actuacionRelevante.Borrado)
            {
                _logger.LogWarning($"request.IdActuacionRelevante: {request.IdActuacionRelevante}, no encontrado");
                throw new NotFoundException(nameof(ActuacionRelevanteDGPCE), request.IdActuacionRelevante);
            }
            return actuacionRelevante;
        }
        else
        {
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (suceso is null || suceso.Borrado)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            return new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
        }
    }

}
