using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
public class ManageEmergenciasNacionalesCommandHandler : IRequestHandler<ManageEmergenciasNacionalesCommand, ManageEmergenciaNacionalResponse>
{

    private readonly ILogger<ManageEmergenciasNacionalesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageEmergenciasNacionalesCommandHandler(
        ILogger<ManageEmergenciasNacionalesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageEmergenciaNacionalResponse> Handle(ManageEmergenciasNacionalesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageEmergenciasNacionalesCommandHandler)} - BEGIN");

        ActuacionRelevanteDGPCE actuacion;
        // Si el ActuacionRelevanteDGPCE es proporcionado, intentar actualizar, si no, crear nueva instancia
        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante.Value > 0)
        {
            var spec = new ActuacionRelevanteDGPCESpecification(request.IdActuacionRelevante.Value);
            actuacion = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(spec);
            if (actuacion is null || actuacion.Borrado)
            {
                _logger.LogWarning($"request.IdActuacionRelevante: {request.IdActuacionRelevante}, no encontrado");
                throw new NotFoundException(nameof(ActuacionRelevanteDGPCE), request.IdActuacionRelevante);
            }
        }
        else
        {
            // Validar si el IdSuceso es válido
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (suceso is null || suceso.Borrado)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            // Crear nueva Actuacion relevante
            actuacion = new ActuacionRelevanteDGPCE
            {
                IdSuceso = request.IdSuceso
            };
        }

        // Mapear la emergencia nacional
        if (request.EmergenciaNacional != null)
        {
            _mapper.Map(request, actuacion, typeof(ManageEmergenciasNacionalesCommand), typeof(ActuacionRelevanteDGPCE));
            actuacion.EmergenciaNacional!.Borrado = false;
            actuacion.EmergenciaNacional.FechaEliminacion = null;
            actuacion.EmergenciaNacional.EliminadoPor = null;

        }
        else if (actuacion.EmergenciaNacional != null && !actuacion.EmergenciaNacional.Borrado)
        {
            // Eliminar lógicamente la emergencia nacional si no se envía en la solicitud
            _unitOfWork.Repository<EmergenciaNacional>().DeleteEntity(actuacion.EmergenciaNacional);
        }

        // Guardar la actuación relevante
        if (request.IdActuacionRelevante.HasValue)
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacion);
        }
        else
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().AddEntity(actuacion);
        }

        var saveResult = await _unitOfWork.Complete();
        if (saveResult <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar nueva emergencia nacional");
        }

        _logger.LogInformation($"{nameof(ManageEmergenciasNacionalesCommandHandler)} - END");
        return new ManageEmergenciaNacionalResponse { IdActuacionRelevante = actuacion.Id };
    }

}
