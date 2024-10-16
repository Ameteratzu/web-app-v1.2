using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosEvolucion.Queries.GetEstadosEvolucionList
{
    public class GetEstadosEvolucionListQueryHandler : IRequestHandler<GetEstadosEvolucionListQuery, IReadOnlyList<EstadoSuceso>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEstadosEvolucionListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<EstadoSuceso>> Handle(GetEstadosEvolucionListQuery request, CancellationToken cancellationToken)
        {
            var estadosEvolucion = await _unitOfWork.Repository<EstadoSuceso>().GetAllAsync();
            return estadosEvolucion;
        }
    }
}
