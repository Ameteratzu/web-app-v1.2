using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
public class GetDescripcionImpactosListQueryHandler : IRequestHandler<GetDescripcionImpactosListQuery, IReadOnlyList<ImpactoVm>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDescripcionImpactosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ImpactoVm>> Handle(GetDescripcionImpactosListQuery request, CancellationToken cancellationToken)
    {

        IReadOnlyList<ImpactoVm> impactoVms = await _unitOfWork.Repository<ImpactoClasificado>().GetAsync(
            predicate: i => (
                (string.IsNullOrEmpty(request.Tipo) || i.TipoImpacto.Contains(request.Tipo)) &&
                (string.IsNullOrEmpty(request.Grupo) || i.GrupoImpacto.Contains(request.Grupo))
                ),
            selector: i => new ImpactoVm
            {
                Id = i.Id,
                Descripcion = i.Descripcion,
            },
            disableTracking: true
            );

        return impactoVms;
    }
}
