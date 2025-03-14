using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetTiposImpactosList;
public class GetTiposImpactosListQueryHandler : IRequestHandler<GetTiposImpactosListQuery, IReadOnlyList<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTiposImpactosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<string>> Handle(GetTiposImpactosListQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<string> tiposImpactosList =
            await _unitOfWork.Repository<ImpactoClasificado>()
            .GetAsync(selector: t => t.TipoImpacto,distinct: true, disableTracking: true);


        return tiposImpactosList;
    }
}
