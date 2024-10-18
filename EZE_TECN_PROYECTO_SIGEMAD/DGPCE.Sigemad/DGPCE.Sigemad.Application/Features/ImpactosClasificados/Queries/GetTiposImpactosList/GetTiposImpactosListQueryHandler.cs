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
        IReadOnlyList<ImpactoClasificado> tiposImpactos = await _unitOfWork.Repository<ImpactoClasificado>().GetAllAsync() ?? new List<ImpactoClasificado>();
        var tiposImpactosList = tiposImpactos
            .Select(t => t.TipoImpacto)
            .Distinct()
            .ToList();

        return tiposImpactosList.AsReadOnly();
    }
}
