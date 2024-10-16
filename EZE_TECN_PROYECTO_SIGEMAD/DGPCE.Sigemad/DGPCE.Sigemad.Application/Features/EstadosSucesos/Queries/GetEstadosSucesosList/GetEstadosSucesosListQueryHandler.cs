using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EstadosSucesos.Queries.GetEstadosSucesosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosEvolucion.Queries.GetEstadosSucesosList
{
    public class GetEstadosSucesosListQueryHandler : IRequestHandler<GetEstadosSucesosListQuery, IReadOnlyList<EstadoSuceso>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEstadosSucesosListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<EstadoSuceso>> Handle(GetEstadosSucesosListQuery request, CancellationToken cancellationToken)
        {
            var estadosSucesos = await _unitOfWork.Repository<EstadoSuceso>().GetAllAsync();
            return estadosSucesos;
        }
    }
}
