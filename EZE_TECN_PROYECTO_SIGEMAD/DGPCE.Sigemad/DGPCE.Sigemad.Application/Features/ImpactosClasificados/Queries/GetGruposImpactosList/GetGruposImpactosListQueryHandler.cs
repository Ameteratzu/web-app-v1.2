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


        if (string.IsNullOrEmpty(request.Tipo))
        {
            return new List<string>();
        }

        IReadOnlyList<string> grupos = await _unitOfWork.Repository<ImpactoClasificado>()
            .GetAsync(selector: t => t.GrupoImpacto, distinct: true, disableTracking: true);

        return grupos;
    }
}
