using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.ClasificacionMedios.Quereis.GetClasificacionMediosList;
public class GetClasificacionMediosListQueryHandler : IRequestHandler<GetClasificacionMediosListQuery, IReadOnlyList<ClasificacionMedio>>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetClasificacionMediosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ClasificacionMedio>> Handle(GetClasificacionMediosListQuery request, CancellationToken cancellationToken)
    {
        var clasificacionMedios = await _unitOfWork.Repository<ClasificacionMedio>().GetAllAsync();
        return clasificacionMedios;
    }
}
