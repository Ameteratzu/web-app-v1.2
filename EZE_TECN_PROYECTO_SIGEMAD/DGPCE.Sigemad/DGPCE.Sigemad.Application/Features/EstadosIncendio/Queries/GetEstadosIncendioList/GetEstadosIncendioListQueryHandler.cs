using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadosIncendioList
{
    public class GetEstadosIncendioListQueryHandler : IRequestHandler<GetEstadosIncendioListQuery, IReadOnlyList<EstadoIncendio>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEstadosIncendioListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<EstadoIncendio>> Handle(GetEstadosIncendioListQuery request, CancellationToken cancellationToken)
        {
            var estadosIncendio = await _unitOfWork.Repository<EstadoIncendio>().GetAllAsync();
            return estadosIncendio;
        }
    }
}
