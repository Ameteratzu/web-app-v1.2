﻿using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.UpdateOpePeriodos;

public class UpdateOpePeriodoCommandHandler : IRequestHandler<UpdateOpePeriodoCommand>
{
    private readonly ILogger<UpdateOpePeriodoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOpePeriodoCommandHandler(
        ILogger<UpdateOpePeriodoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOpePeriodoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpePeriodoCommandHandler) + " - BEGIN");

        var opePeriodoSpec = new OpePeriodoActiveByIdSpecification(request.Id);
        var opePeriodoToUpdate = await _unitOfWork.Repository<OpePeriodo>().GetByIdWithSpec(opePeriodoSpec);


        if (opePeriodoToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope periodo con id: {request.Id}");
            throw new NotFoundException(nameof(OpePeriodo), request.Id);
        }

        // TEST
        //Auditoria auditoria = new Auditoria("Ope_Periodo", AuditoriaConstantes.OperacionModificacion, null);
        //auditoria.ValoresAntiguos = opePeriodoToUpdate.ToAuditoria();
        // FIN TEST

        _mapper.Map(request, opePeriodoToUpdate, typeof(UpdateOpePeriodoCommand), typeof(OpePeriodo));

        // TEST
        //auditoria.ValoresNuevos = opePeriodoToUpdate.ToAuditoria();
        // FIN TEST

        _unitOfWork.Repository<OpePeriodo>().UpdateEntity(opePeriodoToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope periodo con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpePeriodoCommandHandler) + " - END");

        return Unit.Value;
    }
}
