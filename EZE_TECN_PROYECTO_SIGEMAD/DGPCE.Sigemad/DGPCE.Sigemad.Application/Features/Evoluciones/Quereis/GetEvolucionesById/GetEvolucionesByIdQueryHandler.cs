using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
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
            var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdAsync(request.Id);
            var evolucionVm = _mapper.Map<Evolucion, EvolucionVm>(evolucion);
            return evolucionVm;
        }
    }
}

