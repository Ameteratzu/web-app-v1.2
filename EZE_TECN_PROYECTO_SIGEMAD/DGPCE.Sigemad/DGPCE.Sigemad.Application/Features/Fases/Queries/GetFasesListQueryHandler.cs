using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Fases.Queries;

public class GetFasesListQueryHandler : IRequestHandler<GetFasesListQuery, IReadOnlyList<Fase>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFasesListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<Fase>> Handle(GetFasesListQuery request, CancellationToken cancellationToken)
    {
        var fases = await _unitOfWork.Repository<Fase>().GetAllAsync();
        return fases;
    }
}