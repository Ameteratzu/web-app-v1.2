using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
public class ManageActivacionSistemaCommandHandler : IRequestHandler<ManageActivacionSistemaCommand, ManageActivacionSistemaResponse>
{

    private readonly ILogger<ManageActivacionSistemaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageActivacionSistemaCommandHandler(
        ILogger<ManageActivacionSistemaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageActivacionSistemaResponse> Handle(ManageActivacionSistemaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageActivacionSistemaCommandHandler)} - BEGIN");

        var actuacionRelevante = await GetOrCreateActuacionRelevante(request);

        await ValidateTipoSistemaEmergencia(request);
        await ValidateModosActivacion(request);

        await MapAndSaveActivacionSistema(request, actuacionRelevante);

        await DeleteLogicalActivaciones(request, actuacionRelevante);
        await SaveActuacionRelevante(request, actuacionRelevante);

        _logger.LogInformation($"{nameof(ManageActivacionPlanEmergenciaCommandHandler)} - END");
        return new ManageActivacionSistemaResponse { IdActuacionRelevante = actuacionRelevante.Id };
    }


    private async Task DeleteLogicalActivaciones(ManageActivacionSistemaCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante > 0)
        {
            var idsEnRequest = request.Detalles
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var itemsParaEliminar = actuacionRelevante.ActivacionSistemas
                .Where(c => c.Id > 0 && c.Borrado == false && !idsEnRequest.Contains(c.Id))
                .ToList();

            foreach (var detalleAEliminar in itemsParaEliminar)
            {
                _unitOfWork.Repository<ActivacionSistema>().DeleteEntity(detalleAEliminar);
            }
        }
    }


    private async Task SaveActuacionRelevante(ManageActivacionSistemaCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
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
            throw new Exception("No se pudo insertar/actualizar los detalles de la activacion planes");
        }
    }


    private async Task MapAndSaveActivacionSistema(ManageActivacionSistemaCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        foreach (var activacionSistemaDto in request.Detalles)
        {
            bool crearNueva = true;

            if (activacionSistemaDto.Id.HasValue && activacionSistemaDto.Id > 0)
            {
                var activacionSistema = actuacionRelevante.ActivacionSistemas!.FirstOrDefault(a => a.Id == activacionSistemaDto.Id.Value);
                if (activacionSistema != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(activacionSistemaDto, activacionSistema);
                    activacionSistema.Borrado = false;
                    crearNueva = false;
                }
            }

            if (crearNueva)
            {
                // Crear nueva declaracion ZAGEP
                var nuevaActivacionSistema = _mapper.Map<ActivacionSistema>(activacionSistemaDto);
                nuevaActivacionSistema.Id = 0;

                actuacionRelevante.ActivacionSistemas = actuacionRelevante.ActivacionSistemas != null ? actuacionRelevante.ActivacionSistemas : new List<ActivacionSistema>();
                actuacionRelevante.ActivacionSistemas.Add(nuevaActivacionSistema);
            }
        }
    }



    private async Task ValidateModosActivacion(ManageActivacionSistemaCommand request)
    {
        var idsModosActivacion = request.Detalles.Select(c => c.IdModoActivacion).Distinct();
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

    private async Task<ActuacionRelevanteDGPCE> GetOrCreateActuacionRelevante(ManageActivacionSistemaCommand request)
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