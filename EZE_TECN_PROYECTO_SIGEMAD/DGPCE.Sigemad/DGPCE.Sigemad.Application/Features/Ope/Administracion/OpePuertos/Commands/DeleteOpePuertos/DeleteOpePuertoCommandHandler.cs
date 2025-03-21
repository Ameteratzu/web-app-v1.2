﻿using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Commands.DeleteOpePuertos;

public class DeleteOpePuertoCommandHandler : IRequestHandler<DeleteOpePuertoCommand>
{
    private readonly ILogger<DeleteOpePuertoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpePuertoCommandHandler(
        ILogger<DeleteOpePuertoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpePuertoCommand request, CancellationToken cancellationToken)
    {
        var opePuertoToDelete = await _unitOfWork.Repository<OpePuerto>().GetByIdAsync(request.Id);
        if (opePuertoToDelete is null || opePuertoToDelete.Borrado)
        {
            _logger.LogWarning($"El ope puerto con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpePuerto), request.Id);
        }

        _unitOfWork.Repository<OpePuerto>().DeleteEntity(opePuertoToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope puerto con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
