using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using DGPCE.Sigemad.Application.Specifications.AreasAfectadas;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaById;

public class GetAreaAfectadaByIdQueryHandler : IRequestHandler<GetAreaAfectadaByIdQuery, AreaAfectadaVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAreaAfectadaByIdQueryHandler> _logger;

    public GetAreaAfectadaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetAreaAfectadaByIdQueryHandler> logger
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AreaAfectadaVm> Handle(GetAreaAfectadaByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetAreaAfectadaByIdQueryHandler)} - BEGIN");

        var areaSpec = new AreaAfectadaActiveByIdSpecification(request.Id);
        var areaAfectada = await _unitOfWork.Repository<AreaAfectada>().GetByIdWithSpec(areaSpec);

        if (areaAfectada == null)
        {
            _logger.LogWarning($"No se encontro area con id: {request.Id}");
            throw new NotFoundException(nameof(AreaAfectada), request.Id);
        }

        var areaAfectadaVm = _mapper.Map<AreaAfectada, AreaAfectadaVm>(areaAfectada);

        _logger.LogInformation($"{nameof(GetAreaAfectadaByIdQueryHandler)} - END");
        return areaAfectadaVm;
    }
}