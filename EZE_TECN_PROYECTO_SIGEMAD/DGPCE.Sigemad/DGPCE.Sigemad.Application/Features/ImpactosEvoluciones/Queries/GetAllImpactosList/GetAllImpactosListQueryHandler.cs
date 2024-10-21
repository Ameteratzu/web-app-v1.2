using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactosEvolucionesList;
public class GetAllImpactosListQueryHandler : IRequestHandler<GetAllImpactosListQuery, IReadOnlyList<ImpactoEvolucion>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllImpactosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ImpactoEvolucion>> Handle(GetAllImpactosListQuery request, CancellationToken cancellationToken)
    {
        var impactoSpec = new ImpactoEvolucionActiveSpecification();
        var lista = await _unitOfWork.Repository<ImpactoEvolucion>().GetAllWithSpec(impactoSpec);

        return lista;
    }
}
