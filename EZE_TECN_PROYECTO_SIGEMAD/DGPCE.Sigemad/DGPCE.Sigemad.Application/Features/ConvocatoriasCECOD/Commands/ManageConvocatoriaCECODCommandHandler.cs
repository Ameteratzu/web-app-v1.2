using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
public class ManageConvocatoriaCECODCommandHandler : IRequestHandler<ManageConvocatoriaCECODCommand, ManageConvocatoriaCECODResponse>
{
    private readonly ILogger<ManageConvocatoriaCECODCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageConvocatoriaCECODCommandHandler(
        ILogger<ManageConvocatoriaCECODCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ManageConvocatoriaCECODResponse> Handle(ManageConvocatoriaCECODCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageConvocatoriaCECODCommandHandler)} - BEGIN");

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
        foreach (var convocatoriaCecodDto in request.Detalles)
        {
            bool crearNueva = true;

            if (convocatoriaCecodDto.Id.HasValue && convocatoriaCecodDto.Id > 0)
            {
                var convocatoriaCecod = actuacion.ConvocatoriasCECOD!.FirstOrDefault(a => a.Id == convocatoriaCecodDto.Id.Value);
                if (convocatoriaCecod != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(convocatoriaCecodDto, convocatoriaCecod);
                    convocatoriaCecod.Borrado = false;
                    crearNueva = false;
                }
            }

            if (crearNueva)
            {
                // Crear nueva declaracion ZAGEP
                var nuevaConvocatoria = _mapper.Map<ConvocatoriaCECOD>(convocatoriaCecodDto);
                nuevaConvocatoria.Id = 0;

                actuacion.ConvocatoriasCECOD = actuacion.ConvocatoriasCECOD != null ? actuacion.ConvocatoriasCECOD : new List<ConvocatoriaCECOD>();
                actuacion.ConvocatoriasCECOD.Add(nuevaConvocatoria);
            }
        }



        // Eliminar lógicamente las áreas afectadas que no están presentes en el request
        if (request.IdActuacionRelevante.HasValue)
        {
            var idsEnRequest = request.Detalles
                .Where(a => a.Id.HasValue && a.Id > 0)
                .Select(a => a.Id)
                .ToList();

            var convocatoriasParaEliminar = actuacion.ConvocatoriasCECOD!
                .Where(a => a.Id > 0 && !idsEnRequest.Contains(a.Id))
                .ToList();

            foreach (var declaracion in convocatoriasParaEliminar)
            {
                _unitOfWork.Repository<ConvocatoriaCECOD>().DeleteEntity(declaracion);
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

        _logger.LogInformation(nameof(ManageConvocatoriaCECODCommandHandler) + " - END");
        return new ManageConvocatoriaCECODResponse { IdActuacionRelevante = actuacion.Id };
    }

}
