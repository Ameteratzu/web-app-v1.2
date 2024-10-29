using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
internal class GetAreaAfectadaListQueryHandler : IRequestHandler<GetAreaAfectadaListQuery, IReadOnlyList<AreaAfectadaVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAreaAfectadaListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AreaAfectadaVm>> Handle(GetAreaAfectadaListQuery request, CancellationToken cancellationToken)
    {
     
        IReadOnlyList<AreaAfectada> areasAfectadas = (await _unitOfWork.Repository<AreaAfectada>().GetAsync(m => m.IdEvolucion == request.IdEvolucion))
           .OrderBy(m => m.FechaHora)
           .ToList()
           .AsReadOnly();

        var areasAfectadasVm = _mapper.Map<IReadOnlyList<AreaAfectada>, IReadOnlyList<AreaAfectadaVm>>(areasAfectadas);
        return areasAfectadasVm;
    }

   
}
