using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;
public class GetTerritoriosListQueryHandler : IRequestHandler<GetTerritoriosListQuery, IReadOnlyList<Territorio>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTerritoriosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<Territorio>> Handle(GetTerritoriosListQuery request, CancellationToken cancellationToken)
    {
        var territorios = await _unitOfWork.Repository<Territorio>().GetAllAsync();
        return territorios;
    }
}
