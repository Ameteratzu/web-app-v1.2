using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Alertas.Queries.GetEstadosAlertasList;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Alertas;
using DGPCE.Sigemad.Application.Specifications.EstadosAlertas;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Queries.GetEstadosAlertasList
{
    public class GetAlertaByIdQueryHandler : IRequestHandler<GetEstadosAlertasListQuery, PaginationVm<EstadosAlertasVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAlertaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<EstadosAlertasVm>> Handle(GetEstadosAlertasListQuery request, CancellationToken cancellationToken)
        {
            var alertasSpecificationParams = new EstadosAlertasSpecificationParams
            {
                Page = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort,
            };

            var spec = new EstadosAlertasSpecification(alertasSpecificationParams);
            var estadoAlerta = await _unitOfWork.Repository<EstadoAlerta>().GetAllWithSpec(spec);

            var specCount = new EstadosAlertasForCountingSpecification(alertasSpecificationParams);
            var totalAlertas = await _unitOfWork.Repository<EstadoAlerta>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalAlertas) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<EstadoAlerta>, IReadOnlyList<EstadosAlertasVm>>(estadoAlerta);

            var pagination = new PaginationVm<EstadosAlertasVm>
            {
                Count = totalAlertas,
                Data = data,
                PageCount = totalPages,
                Page = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }
    }
}
