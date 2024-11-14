using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Quereis.GetEvolucionesByIdIncendioList
{


    public class GetEvolucionesByIdIncendioListQueryHandler : IRequestHandler<GetEvolucionesByIdIncendioListQuery, IReadOnlyList<EvolucionVm>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEvolucionesByIdIncendioListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<IReadOnlyList<EvolucionVm>> Handle(GetEvolucionesByIdIncendioListQuery request, CancellationToken cancellationToken)
        {

            var evolucionParams = new EvolucionSpecificationParams
            {
                IdIncendio = request.IdIncendio        
            };

            var spec = new EvolucionSpecification(evolucionParams);
            var evoluciones = await _unitOfWork.Repository<Evolucion>()
            .GetAllWithSpec(spec);

            var evolucionesVm = _mapper.Map<IReadOnlyList<Evolucion>, IReadOnlyList<EvolucionVm>>(evoluciones);
            return evolucionesVm;

        }
    }
}
