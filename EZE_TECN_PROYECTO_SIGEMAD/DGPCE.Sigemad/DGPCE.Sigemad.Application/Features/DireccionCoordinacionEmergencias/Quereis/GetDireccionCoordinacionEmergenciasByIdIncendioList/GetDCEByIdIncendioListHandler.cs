using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasList;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;



namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasByIdIncendioList;
internal class GetDCEByIdIncendioListHandler : IRequestHandler<GetDCEByIdIncendioListQuery, IReadOnlyList<DireccionCoordinacionEmergenciaVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDCEByIdIncendioListHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }
    public async Task<IReadOnlyList<DireccionCoordinacionEmergenciaVm>> Handle(GetDCEByIdIncendioListQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<DireccionCoordinacionEmergencia, object>>>();
        includes.Add(c => c.ActivacionPlanEmergencia);

        var DireccionCoordinacionEmergencias = (await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetAsync(e => e.IdIncendio == request.IdIncendio, null, includes))
            .ToList()
            .AsReadOnly();

        var DireccionCoordinacionEmergenciasVm = _mapper.Map<IReadOnlyList<DireccionCoordinacionEmergencia>, IReadOnlyList<DireccionCoordinacionEmergenciaVm>>(DireccionCoordinacionEmergencias);
        return DireccionCoordinacionEmergenciasVm;

    }
}
