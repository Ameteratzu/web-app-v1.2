using DGPCE.Sigemad.Application.Contracts.Persistence;
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
        IReadOnlyList<ImpactoEvolucion> lista = await _unitOfWork.Repository<ImpactoEvolucion>().GetAsync
            (
                predicate: i => i.Borrado == false,
                orderBy: null,
                includeString: null,
                disableTracking: true
            );

        return lista;
    }
}
