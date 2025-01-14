using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
public class ManageDeclaracionesZAGEPCommandHandler : IRequestHandler<ManageDeclaracionesZAGEPCommand, ManageDeclaracionZAGEPResponse>
{
    private readonly ILogger<ManageDeclaracionesZAGEPCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageDeclaracionesZAGEPCommandHandler(
        ILogger<ManageDeclaracionesZAGEPCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageDeclaracionZAGEPResponse> Handle(ManageDeclaracionesZAGEPCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageDeclaracionesZAGEPCommandHandler)} - BEGIN");

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

        // Mapear y actualizar/crear las declaraciones ZAGEP
        foreach (var declaracionZagepDto in request.Detalles!)
        {
            if (declaracionZagepDto.Id.HasValue && declaracionZagepDto.Id > 0)
            {
                var declaracionZagep = actuacion.DeclaracionesZAGEP!.FirstOrDefault(a => a.Id == declaracionZagepDto.Id.Value);
                if (declaracionZagep != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(declaracionZagepDto, declaracionZagep);
                    declaracionZagep.Borrado = false;
                }
                else
                {
                    // Crear nueva declaracion ZAGEP
                    var nuevaDeclaracion = _mapper.Map<DeclaracionZAGEP>(declaracionZagepDto);
                    nuevaDeclaracion.Id = 0;
                    actuacion.DeclaracionesZAGEP!.Add(nuevaDeclaracion);
                }
            }
            else
            {
                // Crear nueva declaracion ZAGEP
                var nuevaDeclaracion = _mapper.Map<DeclaracionZAGEP>(declaracionZagepDto);
                nuevaDeclaracion.Id = 0;
                actuacion.DeclaracionesZAGEP!.Add(nuevaDeclaracion);
            }
        }


        // Eliminar lógicamente las áreas afectadas que no están presentes en el request
        if (request.IdActuacionRelevante.HasValue)
        {
            var idsEnRequest = request.Detalles
                .Where(a => a.Id.HasValue && a.Id > 0)
                .Select(a => a.Id)
                .ToList();

            var declaracionesParaEliminar = actuacion.DeclaracionesZAGEP!
                .Where(a => a.Id > 0 && !idsEnRequest.Contains(a.Id))
                .ToList();

            foreach (var declaracion in declaracionesParaEliminar)
            {
                _unitOfWork.Repository<DeclaracionZAGEP>().DeleteEntity(declaracion);
            }
        }

        if (request.IdActuacionRelevante.HasValue)
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacion);
        }
        else
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().AddEntity(actuacion);
        }

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar las declaraciones ZAGEP");
        }

        _logger.LogInformation(nameof(ManageDeclaracionesZAGEPCommandHandler) + " - END");
        return new ManageDeclaracionZAGEPResponse { IdActuacionRelevante = actuacion.Id };
    }
}