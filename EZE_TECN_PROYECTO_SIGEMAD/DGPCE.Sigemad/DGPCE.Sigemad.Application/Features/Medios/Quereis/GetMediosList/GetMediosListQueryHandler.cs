
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Medios.Quereis.GetMediosList
{
    public class GetMediosListQueryHandler : IRequestHandler<GetMediosListQuery, IReadOnlyList<Medio>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMediosListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Medio>> Handle(GetMediosListQuery request, CancellationToken cancellationToken)
        {
            var medios = await _unitOfWork.Repository<Medio>().GetAllAsync();
            return medios;
        }
    }
}

