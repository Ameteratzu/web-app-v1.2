using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.CCAA.Queries.GetCCAAByIdPaisList;
public class GetCCAAByIdPaisListQueryHandler : IRequestHandler<GetCCAAByIdPaisListQuery, IReadOnlyList<ComunidadesAutonomasSinProvinciasVm>>
{
    private readonly ILogger<GetCCAAByIdPaisListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetCCAAByIdPaisListQueryHandler(
        ILogger<GetCCAAByIdPaisListQueryHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ComunidadesAutonomasSinProvinciasVm>> Handle(GetCCAAByIdPaisListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetCCAAByIdPaisListQueryHandler)} - BEGIN");

        IReadOnlyList<ComunidadesAutonomasSinProvinciasVm> result = await _unitOfWork.Repository<Ccaa>().GetAsync
            (
                predicate: c => c.IdPais == request.IdPais,
                selector: c => new ComunidadesAutonomasSinProvinciasVm
                {
                    Id = c.Id,
                    Descripcion = c.Descripcion,
                },
                orderBy: q => q.OrderBy(c => c.Descripcion),
                disableTracking: true
            );

        _logger.LogInformation($"{nameof(GetCCAAByIdPaisListQueryHandler)} - BEGIN");

        return result;

    }
}
