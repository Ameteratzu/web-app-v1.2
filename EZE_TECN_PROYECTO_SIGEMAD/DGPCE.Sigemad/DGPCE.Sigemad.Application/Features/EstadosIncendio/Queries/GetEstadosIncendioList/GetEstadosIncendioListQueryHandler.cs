using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadosIncendioList;

public class GetEstadosIncendioListQueryHandler : IRequestHandler<GetEstadosIncendioListQuery, IReadOnlyList<EstadoIncendio>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetEstadosIncendioListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<EstadoIncendio>> Handle(GetEstadosIncendioListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Estados_Incendios";

        var estadosIncendio = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<EstadoIncendio>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );
        return estadosIncendio == null ? new List<EstadoIncendio>() : estadosIncendio;
    }
}
