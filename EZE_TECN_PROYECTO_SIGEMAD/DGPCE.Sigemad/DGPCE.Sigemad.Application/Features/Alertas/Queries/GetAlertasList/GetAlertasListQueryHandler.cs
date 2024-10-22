using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Alertas.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Alertas;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Alertas.Queries.GetAlertasListByEstado
{
    public class GetAlertaByIdQueryHandler : IRequestHandler<GetAlertasListQuery, PaginationVm<AlertaVm>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAlertaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<AlertaVm>> Handle(GetAlertasListQuery request, CancellationToken cancellationToken)
        {
            var alertasSpecificationParams = new AlertasSpecificationParams
            {
                IdEstado = request.idEstado,
                FechaAlerta = request.fechaAlerta,
                Page = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort,
            };

            var spec = new AlertasSpecification(alertasSpecificationParams);
            var alertas = await _unitOfWork.Repository<Alerta>().GetAllWithSpec(spec);

            var specCount = new AlertasForCountingSpecification(alertasSpecificationParams);
            var totalAlertas = await _unitOfWork.Repository<Alerta>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalAlertas) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Alerta>, IReadOnlyList<AlertaVm>>(alertas);

            var pagination = new PaginationVm<AlertaVm>
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
