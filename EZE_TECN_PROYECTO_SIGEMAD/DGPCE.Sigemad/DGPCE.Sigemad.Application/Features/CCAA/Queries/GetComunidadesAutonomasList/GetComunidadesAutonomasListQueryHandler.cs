using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Features.CCAA.Queries.GetComunidadesAutonomasList
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

            var ComunidadesAutonomas = (await _unitOfWork.Repository<Ccaa>().GetAsync(null, null, includes))
                .OrderBy(c => c.Descripcion)
                .Select(c => new Ccaa
                {

                    Id = c.Id,
                    Descripcion = c.Descripcion,
                    Provincia = c.Provincia.OrderBy(p => p.Descripcion).ToList()
                }
               )

                .ToList()
                .AsReadOnly();

            var comunidadesAutonomasVm = _mapper.Map<IReadOnlyList<Ccaa>, IReadOnlyList<ComunidadesAutonomasVm>>(ComunidadesAutonomas);
            return comunidadesAutonomasVm;

        }
    }
}
