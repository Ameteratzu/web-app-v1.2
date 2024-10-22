using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Intervenciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.DeleteIntervencionMedios;
public class DeleteIntervencionMedioCommandHandler : IRequestHandler<DeleteIntervencionMedioCommand>
{
    private readonly ILogger<DeleteIntervencionMedioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteIntervencionMedioCommandHandler(
        ILogger<DeleteIntervencionMedioCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteIntervencionMedioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(DeleteIntervencionMedioCommandHandler)} - BEGIN");

        var intervencionSpec = new IntervencionActiveByIdSpecification(request.Id);
        var intervencionToUpdate = await _unitOfWork.Repository<IntervencionMedio>().GetByIdWithSpec(intervencionSpec);
        if (intervencionToUpdate == null)
        {
            _logger.LogWarning($"No se encontro intervencion con id: {request.Id}");
            throw new NotFoundException(nameof(IntervencionMedio), request.Id);
        }

        intervencionToUpdate.Borrado = true;
        intervencionToUpdate.FechaEliminacion = DateTime.Now;

        _unitOfWork.Repository<IntervencionMedio>().UpdateEntity(intervencionToUpdate);
        await _unitOfWork.Complete();
        _logger.LogInformation($"La intervencion de medio con id: {request.Id}, se elimino de forma logica con exito");

        _logger.LogInformation($"{nameof(DeleteIntervencionMedioCommandHandler)} - END");
        return Unit.Value;
    }
}
