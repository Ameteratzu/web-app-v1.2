using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.ComparativaFechas.Quereis.GetComparativaFechasList
{
        public class GetComparativaFechasListQueryHandler : IRequestHandler<GetComparativaFechasListQuery, IReadOnlyList<ComparativaFecha>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetComparativaFechasListQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IReadOnlyList<ComparativaFecha>> Handle(GetComparativaFechasListQuery request, CancellationToken cancellationToken)
            {
                var comparativaFechas = await _unitOfWork.Repository<ComparativaFecha>().GetAllAsync();
                return comparativaFechas;
            }
        }
    }

