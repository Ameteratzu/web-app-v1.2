using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis.GetCaracterMediosList;
public class GetCaracterMediosListQueryHandler : IRequestHandler<GetCaracterMediosListQuery, IReadOnlyList<CaracterMedio>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCaracterMediosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CaracterMedio>> Handle(GetCaracterMediosListQuery request, CancellationToken cancellationToken)
    {
        var caracterMedios = await _unitOfWork.Repository<CaracterMedio>().GetAllAsync();
        return caracterMedios;
    }
}

