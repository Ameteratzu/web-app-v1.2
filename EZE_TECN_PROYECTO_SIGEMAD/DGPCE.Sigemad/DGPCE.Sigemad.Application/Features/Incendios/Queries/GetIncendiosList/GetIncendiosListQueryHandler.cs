using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList;

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

        var incendios = await _unitOfWork.Repository<Incendio>().GetAllAsync();

        var pagination = new PaginationVm<Incendio>
        {
            Count = incendios.Count,
            Data = incendios,
            PageCount = incendios.Count,
            Page = 0,
            PageSize = 0
        };

        _logger.LogInformation($"{nameof(GetIncendiosListQueryHandler)} - END");
        return pagination;
    }
}
