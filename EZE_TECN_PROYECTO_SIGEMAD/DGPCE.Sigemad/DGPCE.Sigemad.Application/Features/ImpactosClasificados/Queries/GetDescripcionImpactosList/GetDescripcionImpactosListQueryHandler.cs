using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
public class GetDescripcionImpactosListQueryHandler : IRequestHandler<GetDescripcionImpactosListQuery, IReadOnlyList<ImpactoClasificadoDescripcionVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDescripcionImpactosListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ImpactoClasificadoDescripcionVm>> Handle(GetDescripcionImpactosListQuery request, CancellationToken cancellationToken)
    {
        var impactos = await _unitOfWork.Repository<ImpactoClasificado>().GetAllAsync();

        var impactosList = _mapper.Map<IReadOnlyList<ImpactoClasificado>, IReadOnlyList<ImpactoClasificadoDescripcionVm>>(impactos);

        return impactosList;
    }
}
