using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeDatosFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatosFronterasList
{
    public class GetOpeDatosFronterasListQueryHandler : IRequestHandler<GetOpeDatosFronterasListQuery, PaginationVm<OpeDatoFronteraVm>>
    {
        private readonly ILogger<GetOpeDatosFronterasListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeDatosFronterasListQueryHandler(
        ILogger<GetOpeDatosFronterasListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeDatoFronteraVm>> Handle(GetOpeDatosFronterasListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeDatosFronterasListQueryHandler)} - BEGIN");

            var spec = new OpeDatosFronterasSpecification(request);
            var opeDatosFronteras = await _unitOfWork.Repository<OpeDatoFrontera>()
            .GetAllWithSpec(spec);

            var specCount = new OpeDatosFronterasForCountingSpecification(request);
            var totalOpeDatosFronteras = await _unitOfWork.Repository<OpeDatoFrontera>().CountAsync(specCount);
            var opeDatoFronteraVmList = _mapper.Map<List<OpeDatoFronteraVm>>(opeDatosFronteras);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeDatosFronteras) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeDatoFronteraVm>
            {
                Count = totalOpeDatosFronteras,
                Data = opeDatoFronteraVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeDatosFronterasListQueryHandler)} - END");
            return pagination;
        }


    }
}