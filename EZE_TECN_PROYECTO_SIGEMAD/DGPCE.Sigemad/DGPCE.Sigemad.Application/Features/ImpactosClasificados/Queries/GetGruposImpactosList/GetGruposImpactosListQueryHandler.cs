using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetGruposImpactosList;
public class GetGruposImpactosListQueryHandler : IRequestHandler<GetGruposImpactosListQuery, IReadOnlyList<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGruposImpactosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<string>> Handle(GetGruposImpactosListQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<ImpactoClasificado> gruposImpactos = await _unitOfWork.Repository<ImpactoClasificado>()
            .GetAllAsync() ?? new List<ImpactoClasificado>();

        var gruposImpactosList = gruposImpactos
            .Select(t => t.GrupoImpacto)
            .Distinct()
            .ToList();

        return gruposImpactosList.AsReadOnly();
    }
}
