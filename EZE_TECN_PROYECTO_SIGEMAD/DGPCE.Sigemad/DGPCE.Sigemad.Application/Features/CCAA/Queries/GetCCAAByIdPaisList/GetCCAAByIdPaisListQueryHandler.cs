using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.CCAA.Queries.GetCCAAByIdPaisList;
public class GetCCAAByIdPaisListQueryHandler : IRequestHandler<GetCCAAByIdPaisListQuery, IReadOnlyList<ComunidadesAutonomasSinProvinciasVm>>
{
    private readonly ILogger<GetCCAAByIdPaisListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCCAAByIdPaisListQueryHandler(
        ILogger<GetCCAAByIdPaisListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ComunidadesAutonomasSinProvinciasVm>> Handle(GetCCAAByIdPaisListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetCCAAByIdPaisListQueryHandler)} - BEGIN");

        var pais = await _unitOfWork.Repository<Pais>().GetByIdAsync(request.IdPais);
        if (pais is null)
        {
            _logger.LogWarning($"request.IdPais: {request.IdPais}, no encontrado");
            throw new NotFoundException(nameof(Pais), request.IdPais);
        }

        var lista = await _unitOfWork.Repository<Ccaa>().GetAsync( c => c.IdPais == request.IdPais );

        _logger.LogInformation($"{nameof(GetCCAAByIdPaisListQueryHandler)} - BEGIN");

        return _mapper.Map<IReadOnlyList<ComunidadesAutonomasSinProvinciasVm>>(lista);

    }
}
