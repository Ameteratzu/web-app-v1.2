using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ActividadesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
public class ManageActivacionPlanEmergenciaCommandHandler : IRequestHandler<ManageActivacionPlanEmergenciaCommand, ManageActivacionPlanEmergenciaResponse>
{
    private readonly ILogger<ManageActivacionPlanEmergenciaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private const string ARCHIVOS_PATH = "activacion-plan-emergencia";

    public ManageActivacionPlanEmergenciaCommandHandler(
        ILogger<ManageActivacionPlanEmergenciaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<ManageActivacionPlanEmergenciaResponse> Handle(ManageActivacionPlanEmergenciaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageActivacionPlanEmergenciaCommandHandler)} - BEGIN");

        ActuacionRelevanteDGPCE actuacionRelevante;

        // Si el IdActuacionRelevante es proporcionado, intentar actualizar, si no, crear nueva instancia
        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante.Value > 0)
        {
            var spec = new ActuacionRelevanteDGPCESpecification(request.IdActuacionRelevante.Value);
            actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(spec);
            if (actuacionRelevante is null || actuacionRelevante.Borrado)
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

            // Crear nueva ActuacionRelevanteDGPCE
            actuacionRelevante = new ActuacionRelevanteDGPCE
            {
                IdSuceso = request.IdSuceso
            };
        }

        // Validar listas
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

        var idsPlanesEmergencias = request.ActivacionesPlanes.Select(c => c.IdPlanEmergencia).Distinct();
        var planesEmergenciasExistentes = await _unitOfWork.Repository<PlanEmergencia>().GetAsync(p => idsPlanesEmergencias.Contains(p.Id));

        if (planesEmergenciasExistentes.Count() != idsPlanesEmergencias.Count())
        {
            var idsPlanesEmergenciasExistentes = planesEmergenciasExistentes.Select(p => p.Id).Cast<int?>().ToList();
            var idsPlanesEmergenciasInvalidas = idsTipoPlan.Except(idsPlanesEmergenciasExistentes).ToList();

            if (idsPlanesEmergenciasInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Planes Emergencias: {string.Join(", ", idsPlanesEmergenciasInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(PlanEmergencia), string.Join(", ", idsPlanesEmergenciasInvalidas));
            }
        }




        // Mapear y actualizar/crear los detalles del documento
        foreach (var activacionPlanDto in request.ActivacionesPlanes)
        {
            if (activacionPlanDto.Id.HasValue && activacionPlanDto.Id > 0)
            {
                var activacionPlanEntity = actuacionRelevante.ActivacionPlanEmergencias.FirstOrDefault(c => c.Id == activacionPlanDto.Id.Value);

                if (activacionPlanEntity != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(activacionPlanDto, activacionPlanEntity);
                    activacionPlanEntity.Borrado = false;

                }
                else
                {
                    // Crear nuevo 
                    var nuevoActivacionPlan = new ActivacionPlanEmergencia
                    {
                        IdTipoPlan = activacionPlanDto.IdTipoPlan,
                        IdPlanEmergencia = activacionPlanDto.IdPlanEmergencia,
                        TipoPlanPersonalizado = activacionPlanDto.TipoPlanPersonalizado,
                        PlanEmergenciaPersonalizado = activacionPlanDto.PlanEmergenciaPersonalizado,
                        FechaInicio = activacionPlanDto.FechaInicio,
                        FechaFin = activacionPlanDto.FechaFin,
                        Autoridad = activacionPlanDto.Autoridad,
                        Observaciones = activacionPlanDto.Observaciones,

                    };


                    // Agregar archivo - BEGIN
                    if (activacionPlanDto.Archivo != null)
                    {
                        var fileEntity = new Archivo
                        {
                            NombreOriginal = activacionPlanDto.Archivo?.FileName ?? string.Empty,
                            NombreUnico = $"{Path.GetFileNameWithoutExtension(activacionPlanDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{activacionPlanDto.Archivo?.Extension ?? string.Empty}",
                            Tipo = activacionPlanDto.Archivo?.ContentType ?? string.Empty,
                            Extension = activacionPlanDto.Archivo?.Extension ?? string.Empty,
                            PesoEnBytes = activacionPlanDto.Archivo?.Length ?? 0,
                        };

                        fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(activacionPlanDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
                        fileEntity.FechaCreacion = DateTime.Now;
                        nuevoActivacionPlan.Archivo = fileEntity;
                    }
                    // Agregar archivo - END

                    actuacionRelevante.ActivacionPlanEmergencias.Add(nuevoActivacionPlan);
                }
            }
            else
            {
                // Crear nuevo 
                var nuevoActivacionPlan = new ActivacionPlanEmergencia
                {
                    IdTipoPlan = activacionPlanDto.IdTipoPlan,
                    IdPlanEmergencia = activacionPlanDto.IdPlanEmergencia,
                    TipoPlanPersonalizado = activacionPlanDto.TipoPlanPersonalizado,
                    PlanEmergenciaPersonalizado = activacionPlanDto.PlanEmergenciaPersonalizado,
                    FechaInicio = activacionPlanDto.FechaInicio,
                    FechaFin = activacionPlanDto.FechaFin,
                    Autoridad = activacionPlanDto.Autoridad,
                    Observaciones = activacionPlanDto.Observaciones,

                };

                // Agregar archivo - BEGIN
                if (activacionPlanDto.Archivo != null)
                {
                    var fileEntity = new Archivo
                    {
                        NombreOriginal = activacionPlanDto.Archivo?.FileName ?? string.Empty,
                        NombreUnico = $"{Path.GetFileNameWithoutExtension(activacionPlanDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{activacionPlanDto.Archivo?.Extension ?? string.Empty}",
                        Tipo = activacionPlanDto.Archivo?.ContentType ?? string.Empty,
                        Extension = activacionPlanDto.Archivo?.Extension ?? string.Empty,
                        PesoEnBytes = activacionPlanDto.Archivo?.Length ?? 0,
                    };

                    fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(activacionPlanDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
                    fileEntity.FechaCreacion = DateTime.Now;
                    nuevoActivacionPlan.Archivo = fileEntity;
                }
                // Agregar archivo - END

                actuacionRelevante.ActivacionPlanEmergencias.Add(nuevoActivacionPlan);
            }
        }


        // Eliminar lógicamente 
        if (request.IdActuacionRelevante.HasValue)
        {
            // Solo las activaciones con Id existente (no nuevas)
            var idsEnRequest = request.ActivacionesPlanes
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var itemsParaEliminar = actuacionRelevante.ActivacionPlanEmergencias
                .Where(c => c.Id > 0 && c.Borrado == false && !idsEnRequest.Contains(c.Id))
                .ToList();

            foreach (var detalleAEliminar in itemsParaEliminar)
            {
                if (detalleAEliminar.IdArchivo.HasValue)
                {
                    _unitOfWork.Repository<Archivo>().DeleteEntity(detalleAEliminar.Archivo);
                }

                _unitOfWork.Repository<ActivacionPlanEmergencia>().DeleteEntity(detalleAEliminar);
            }
        }

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
            throw new Exception("No se pudo insertar/actualizar los detalles de la activacion planes");
        }


        _logger.LogInformation($"{nameof(ManageActivacionPlanEmergenciaCommandHandler)} - END");
        return new ManageActivacionPlanEmergenciaResponse { IdActuacionRelevante = actuacionRelevante.Id };
    }
}
