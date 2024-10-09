using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries;

public class GetIncendiosListQueryHandler : IRequestHandler<GetIncendiosListQuery, PaginationVm<Incendio>>
{
    private readonly ILogger<GetIncendiosListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetIncendiosListQueryHandler(
        ILogger<GetIncendiosListQueryHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginationVm<Incendio>> Handle(GetIncendiosListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetIncendiosListQueryHandler)} - BEGIN");

        var spec = new IncendiosSpecification(request);
        var incendios = await _unitOfWork.Repository<Incendio>()
        .GetAllWithSpec(spec);

        var specCount = new IncendiosForCountingSpecification(request);
        var totalIncendios = await _unitOfWork.Repository<Incendio>().CountAsync(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalIncendios) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var pagination = new PaginationVm<Incendio>
        {
            Count = totalIncendios,
            Data = incendios,
            PageCount = totalPages,
            Page = request.Page,
            PageSize = request.PageSize
        };

        _logger.LogInformation($"{nameof(GetIncendiosListQueryHandler)} - END");
        return pagination;
    }
}
