using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoPlanes.Quereis.GetTipoPlanesList;
public class GetTipoPlanesListQueryHandler : IRequestHandler<GetTipoPlanesListQuery, IReadOnlyList<TipoPlan>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTipoPlanesListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TipoPlan>> Handle(GetTipoPlanesListQuery request, CancellationToken cancellationToken)
    {
        var tipoPlanes = await _unitOfWork.Repository<TipoPlan>().GetAllAsync();
        return tipoPlanes;
    }
}