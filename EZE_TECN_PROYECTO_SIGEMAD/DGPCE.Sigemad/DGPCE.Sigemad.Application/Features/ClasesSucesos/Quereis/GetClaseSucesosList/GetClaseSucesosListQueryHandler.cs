using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.ClasesSucesos.Quereis.GetClaseSucesosList
{
   
        public class GetClaseSucesosListQueryHandler : IRequestHandler<GetClaseSucesosListQuery, IReadOnlyList<ClaseSuceso>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetClaseSucesosListQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IReadOnlyList<ClaseSuceso>> Handle(GetClaseSucesosListQuery request, CancellationToken cancellationToken)
            {
                var claseSucesos = await _unitOfWork.Repository<ClaseSuceso>().GetAllAsync();
                return claseSucesos;
            }
        }
    }

