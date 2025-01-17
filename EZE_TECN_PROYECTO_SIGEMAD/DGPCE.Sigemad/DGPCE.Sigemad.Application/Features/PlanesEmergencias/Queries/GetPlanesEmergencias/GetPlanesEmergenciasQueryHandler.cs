using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications.PlanesEmergencias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergencias;
public class GetPlanesEmergenciasQueryHandler : IRequestHandler<GetPlanesEmergenciasQuery, IReadOnlyList<PlanEmergenciaVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetPlanesEmergenciasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<PlanEmergenciaVm>> Handle(GetPlanesEmergenciasQuery request, CancellationToken cancellationToken)
    {
        var spec = new PlanesEmergenciasSpecification(request);
        var planesEmergencias = await _unitOfWork.Repository<PlanEmergencia>().GetAllWithSpec(spec);

        var planesEmergenciaVm = _mapper.Map<IReadOnlyList<PlanEmergencia>, IReadOnlyList<PlanEmergenciaVm>>(planesEmergencias, opt =>
        {
            opt.Items["IsFullDescription"] = request.IsFullDescription;
        });
        return planesEmergenciaVm;
    }
}
