using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Paises.Queries.GetPaisesList
{
    public class GetPaisesListQueryHandler : IRequestHandler<GetPaisesListQuery, IReadOnlyList<Pais>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPaisesListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Pais>> Handle(GetPaisesListQuery request, CancellationToken cancellationToken)
        {
            var paises = await _unitOfWork.Repository<Pais>().GetAllAsync();
            return paises;
        }
    }
}
