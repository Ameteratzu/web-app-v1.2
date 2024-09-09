using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Alertas.Queries.GetEstadosAlertasList;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Queries.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Alertas;
using DGPCE.Sigemad.Application.Specifications.ComunidadesAutonomas;
using DGPCE.Sigemad.Application.Specifications.EstadosAlertas;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList
{
    public class GetComunidadesAutonomasHandler : IRequestHandler<GetComunidadesAutonomasListQuery, PaginationVm<ComunidadesAutonomasVm>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetComunidadesAutonomasHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationVm<ComunidadesAutonomasVm>> Handle(GetComunidadesAutonomasListQuery request, CancellationToken cancellationToken)
        {

            var comunidadesSpecificationParams = new ComunidadesAutonomasSpecificationParams
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort,
            };

            var spec = new ComunidadesAutonomasSpecification(comunidadesSpecificationParams);
            var comunidadAutonoma = await _unitOfWork.Repository<ComunidadAutonoma>().GetAllWithSpec(spec);

            var specCount = new ComunidadesAutonomasForCountingSpecification(comunidadesSpecificationParams);
            var totalAlertas = await _unitOfWork.Repository<ComunidadAutonoma>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalAlertas) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<ComunidadAutonoma>, IReadOnlyList<ComunidadesAutonomasVm>>(comunidadAutonoma);

            var pagination = new PaginationVm<ComunidadesAutonomasVm>
            {
                Count = totalAlertas,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;

        }
    }
}
