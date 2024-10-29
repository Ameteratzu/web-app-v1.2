using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaById;

public class GetAreaAfectadaBydQuerHandler : IRequestHandler<GetAreaAfectadaByIdQuery, AreaAfectadaVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAreaAfectadaBydQuerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AreaAfectadaVm> Handle(GetAreaAfectadaByIdQuery request, CancellationToken cancellationToken)
    {
        var areaAfectada = await _unitOfWork.Repository<AreaAfectada>().GetByIdAsync(request.Id);
        var evolucionVm = _mapper.Map<AreaAfectada, AreaAfectadaVm>(areaAfectada);
        return evolucionVm;
    }
}