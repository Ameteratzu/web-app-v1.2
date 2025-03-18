using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ModosActivacion.Queries.GetModosActivacionList;

    public class GetModosActivacionListQueryHandler : IRequestHandler<GetModosActivacionListQuery, IReadOnlyList<ModoActivacion>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetModosActivacionListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ModoActivacion>> Handle(GetModosActivacionListQuery request, CancellationToken cancellationToken)
        {
            var modosActivacion = await _unitOfWork.Repository<ModoActivacion>().GetAllNoTrackingAsync();
            return modosActivacion;
        }
    }
