using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList
{
    public class GetComunidadesAutonomasListQueryHandler : IRequestHandler<GetComunidadesAutonomasListQuery, IReadOnlyList<Ccaa>>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetComunidadesAutonomasListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Ccaa>> Handle(GetComunidadesAutonomasListQuery request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Ccaa, object>>>();
            includes.Add(c => c.Provincia);

            var ComunidadesAutonomas = await _unitOfWork.Repository<Ccaa>().GetAsync(null,null,includes);
            return ComunidadesAutonomas;

        }
    }
}
