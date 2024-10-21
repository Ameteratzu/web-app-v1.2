using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactosByEvolucionIdList;
public class GetImpactosByEvolucionIdListQueryHandler : IRequestHandler<GetImpactosByEvolucionIdListQuery, IReadOnlyList<ImpactoEvolucion>>
{
    private readonly ILogger<GetImpactosByEvolucionIdListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public GetImpactosByEvolucionIdListQueryHandler(
        ILogger<GetImpactosByEvolucionIdListQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ImpactoEvolucion>> Handle(GetImpactosByEvolucionIdListQuery request, CancellationToken cancellationToken)
    {
        var evolucionSpec = new EvolucionActiveByIdSpecification(request.IdEvolucion);
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);
        if (evolucion == null)
        {
            _logger.LogWarning($"No se encontro evolucion con id: {request.IdEvolucion}");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
        }

        var impactoSpec = new ImpactoActiveByIdEvolucionSpecification(request.IdEvolucion);
        var impactos = await _unitOfWork.Repository<ImpactoEvolucion>().GetAllWithSpec(impactoSpec);
        return impactos;
    }
}
