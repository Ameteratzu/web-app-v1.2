using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.SituacionesEquivalentes.Queries;



public class GetSituacionesEquivalentesListQueryHandler : IRequestHandler<GetSituacionesEquivalentesListQuery, IReadOnlyList<SituacionEquivalente>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSituacionesEquivalentesListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<SituacionEquivalente>> Handle(GetSituacionesEquivalentesListQuery request, CancellationToken cancellationToken)
    {
        var situacionesEquivalentes = await _unitOfWork.Repository<SituacionEquivalente>().GetAllAsync();
        return situacionesEquivalentes;
    }
}