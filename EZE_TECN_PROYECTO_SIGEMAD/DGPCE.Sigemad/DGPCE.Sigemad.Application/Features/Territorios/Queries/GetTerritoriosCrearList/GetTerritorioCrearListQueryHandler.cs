using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosCrearList;
public class GetTerritorioCrearListQueryHandler : IRequestHandler<GetTerritorioCrearListQuery, IReadOnlyList<TerritorioVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTerritorioCrearListQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TerritorioVm>> Handle(GetTerritorioCrearListQuery request, CancellationToken cancellationToken)
    {
        var territorios = await _unitOfWork.Repository<Territorio>().GetAllAsync();
        territorios = territorios.Where(t => t.Comun == true).ToList();

        return _mapper.Map<IReadOnlyList<TerritorioVm>>(territorios);
    }
}
