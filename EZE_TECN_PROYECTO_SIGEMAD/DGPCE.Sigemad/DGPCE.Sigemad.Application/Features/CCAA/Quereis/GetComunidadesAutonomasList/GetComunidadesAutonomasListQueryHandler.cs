using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Alertas.Queries.Vms;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList
{
    public class GetComunidadesAutonomasListQueryHandler : IRequestHandler<GetComunidadesAutonomasListQuery, IReadOnlyList<ComunidadesAutonomasVm>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetComunidadesAutonomasListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<IReadOnlyList<ComunidadesAutonomasVm>> Handle(GetComunidadesAutonomasListQuery request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Ccaa, object>>>();
            includes.Add(c => c.Provincia);
            
            var ComunidadesAutonomas = await _unitOfWork.Repository<Ccaa>().GetAsync(null,null,includes);

            var comunidadesAutonomasVm = _mapper.Map<IReadOnlyList<Ccaa>, IReadOnlyList<ComunidadesAutonomasVm>>(ComunidadesAutonomas);
            return comunidadesAutonomasVm;

        }
    }
}
