using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactoEvolucionById;
public class GetImpactoByIdQueryHandler : IRequestHandler<GetImpactoByIdQuery, ImpactoEvolucion>
{
    private readonly ILogger<GetImpactoByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetImpactoByIdQueryHandler(
        ILogger<GetImpactoByIdQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ImpactoEvolucion> Handle(GetImpactoByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetImpactoByIdQueryHandler)} - BEGIN");

        var impactoSpec = new ImpactoActiveByIdSpecification(request.Id);
        var impactoEvolucion = await _unitOfWork.Repository<ImpactoEvolucion>().GetByIdWithSpec(impactoSpec);

        if (impactoEvolucion == null)
        {
            _logger.LogWarning($"No se encontro impacto con id: {request.Id}");
            throw new NotFoundException(nameof(ImpactoEvolucion), request.Id);
        }

        _logger.LogInformation($"{nameof(GetImpactoByIdQueryHandler)} - END");
        return impactoEvolucion;
    }
}
