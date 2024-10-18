using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var tiposImpactos = await _unitOfWork.Repository<ImpactoClasificado>().GetAllAsync();
        var tiposImpactosList = tiposImpactos
            .Select(t => t.TipoImpacto)
            .Distinct()
            .ToList();

        return tiposImpactosList.AsReadOnly();
    }
}
