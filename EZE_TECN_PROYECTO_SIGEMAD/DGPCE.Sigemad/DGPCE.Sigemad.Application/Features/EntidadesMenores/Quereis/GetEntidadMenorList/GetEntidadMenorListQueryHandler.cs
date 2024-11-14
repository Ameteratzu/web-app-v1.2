using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorList;

public class GetEntidadMenorListQueryHandler : IRequestHandler<GetEntidadMenorListQuery, IReadOnlyList<EntidadMenorVm>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEntidadMenorListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    public async Task<IReadOnlyList<EntidadMenorVm>> Handle(GetEntidadMenorListQuery request, CancellationToken cancellationToken)
    {

        var entidadesMenores = (await _unitOfWork.Repository<EntidadMenor>().GetAllAsync());

        var entidadesMenoresVm = _mapper.Map<IReadOnlyList<EntidadMenor>, IReadOnlyList<EntidadMenorVm>>(entidadesMenores);
        return entidadesMenoresVm;

    }
}