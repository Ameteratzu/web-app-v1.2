using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasList;
public class GetDireccionCoordinacionEmergenciasHandler : IRequestHandler<GetDireccionCoordinacionEmergenciasListQuery, IReadOnlyList<DireccionCoordinacionEmergenciaVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDireccionCoordinacionEmergenciasHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    public async Task<IReadOnlyList<DireccionCoordinacionEmergenciaVm>> Handle(GetDireccionCoordinacionEmergenciasListQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<DireccionCoordinacionEmergencia, object>>>();
        includes.Add(c => c.ActivacionPlanEmergencia);

        var DireccionCoordinacionEmergencias = (await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetAsync(null, null, includes))
            .ToList()
            .AsReadOnly();

        var DireccionCoordinacionEmergenciasVm = _mapper.Map<IReadOnlyList<DireccionCoordinacionEmergencia>, IReadOnlyList<DireccionCoordinacionEmergenciaVm>>(DireccionCoordinacionEmergencias);
        return DireccionCoordinacionEmergenciasVm;

    }
}
