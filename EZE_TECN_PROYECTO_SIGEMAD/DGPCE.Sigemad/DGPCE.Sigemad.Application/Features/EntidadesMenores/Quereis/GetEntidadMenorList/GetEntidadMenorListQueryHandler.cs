using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;


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
        var includes = new List<Expression<Func<EntidadMenor, object>>>();
        includes.Add(e => e.Distrito.Pais);

        var entidadesMenores = (await _unitOfWork.Repository<EntidadMenor>().GetAsync(null, null, includes))
            .ToList()
            .AsReadOnly();

        var entidadesMenoresVm = _mapper.Map<IReadOnlyList<EntidadMenor>, IReadOnlyList<EntidadMenorVm>>(entidadesMenores);
        return entidadesMenoresVm;

    }
}