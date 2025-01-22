using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
public class ManageActivacionSistemaCommandHandler : IRequestHandler<ManageActivacionSistemaCommand, ManageActivacionSistemaResponse>
{

    private readonly ILogger<ManageActivacionSistemaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageActivacionSistemaCommandHandler(
        ILogger<ManageActivacionSistemaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService
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
        //await ValidatePlanesEmergencias(request);
    }


    private async Task ValidateTipoSistemaEmergencia(ManageActivacionSistemaCommand request)
    {
        var idsTipoSistemaEmergencia = request.ActivacionSistemas.Select(c => c.IdTipoSistemaEmergencia).Distinct();
        var tipoSistemaEmergenciaExistentes = await _unitOfWork.Repository<TipoSistemaEmergencia>().GetAsync(p => idsTipoSistemaEmergencia.Contains(p.Id));

        if (tipoSistemaEmergenciaExistentes.Count() != idsTipoSistemaEmergencia.Count())
        {
            var idsTipoSistemaEmergenciasExistentes = tipoSistemaEmergenciaExistentes.Select(p => p.Id).Cast<int?>().ToList();
            var idsTipoSistemaEmergenciasInvalidas = idsTipoSistemaEmergencia.Except(idsTipoSistemaEmergenciasExistentes).ToList();

            if (idsTipoSistemaEmergenciasInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Tipo Plan: {string.Join(", ", idsTipoSistemaEmergenciasInvalidas)}, no se encontraron");
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