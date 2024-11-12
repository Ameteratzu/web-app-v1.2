using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Quereis.GetEvolucionesById
{

         public class GetEvolucionesByIdQueryHandler : IRequestHandler<GetEvolucionesByIdQuery, EvolucionVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEvolucionesByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EvolucionVm> Handle(GetEvolucionesByIdQuery request, CancellationToken cancellationToken)
        {

            var evolucionParams = new EvolucionSpecificationParams
            {
                Id = request.Id
            };

            var spec = new EvolucionSpecification(evolucionParams);
            var evolucion = await _unitOfWork.Repository<Evolucion>()
            .GetByIdWithSpec(spec);

            var evolucionVm = _mapper.Map<Evolucion, EvolucionVm>(evolucion);
            return evolucionVm;
        }
    }
}

