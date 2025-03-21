using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.DeleteOpePeriodos;

public class DeleteOpePeriodoCommandHandler : IRequestHandler<DeleteOpePeriodoCommand>
{
    private readonly ILogger<DeleteOpePeriodoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpePeriodoCommandHandler(
        ILogger<DeleteOpePeriodoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpePeriodoCommand request, CancellationToken cancellationToken)
    {
        var opePeriodoToDelete = await _unitOfWork.Repository<OpePeriodo>().GetByIdAsync(request.Id);
        if (opePeriodoToDelete is null || opePeriodoToDelete.Borrado)
        {
            _logger.LogWarning($"El ope periodo con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpePeriodo), request.Id);
        }

        _unitOfWork.Repository<OpePeriodo>().DeleteEntity(opePeriodoToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope periodo con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
