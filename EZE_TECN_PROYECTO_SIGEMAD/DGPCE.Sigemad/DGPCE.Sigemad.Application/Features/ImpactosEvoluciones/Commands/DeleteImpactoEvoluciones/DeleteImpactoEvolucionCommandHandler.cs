using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.DeleteImpactoEvoluciones;
public class DeleteImpactoEvolucionCommandHandler : IRequestHandler<DeleteImpactoEvolucionCommand>
{
    private readonly ILogger<DeleteImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteImpactoEvolucionCommandHandler(
        ILogger<DeleteImpactoEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteImpactoEvolucionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(DeleteImpactoEvolucionCommandHandler)} - BEGIN");

        var impactoSpec = new ImpactoActiveByIdSpecification(request.Id);
        var impactoEvolucionToUpdate = await _unitOfWork.Repository<ImpactoEvolucion>().GetByIdWithSpec(impactoSpec);
        if (impactoEvolucionToUpdate == null)
        {
            _logger.LogWarning($"No se encontro impacto con id: {request.Id}");
            throw new NotFoundException(nameof(ImpactoEvolucion), request.Id);
        }

        impactoEvolucionToUpdate.Borrado = true;
        impactoEvolucionToUpdate.FechaEliminacion = DateTime.Now;

        _unitOfWork.Repository<ImpactoEvolucion>().UpdateEntity(impactoEvolucionToUpdate);
        await _unitOfWork.Complete();
        _logger.LogInformation($"El impacto-evolucion con id: {request.Id}, se elimino de forma logica con exito");

        _logger.LogInformation($"{nameof(DeleteImpactoEvolucionCommandHandler)} - END");
        return Unit.Value;
    }
}
