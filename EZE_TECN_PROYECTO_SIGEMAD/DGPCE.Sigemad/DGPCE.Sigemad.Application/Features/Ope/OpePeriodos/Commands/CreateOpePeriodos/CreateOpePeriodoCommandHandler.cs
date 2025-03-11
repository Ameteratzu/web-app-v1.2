using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.OpePeriodos.Commands.CreateOpePeriodos;

public class CreateOpePeriodoCommandHandler : IRequestHandler<CreateOpePeriodoCommand, CreateOpePeriodoResponse>
{
    private readonly ILogger<CreateOpePeriodoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOpePeriodoCommandHandler(
        ILogger<CreateOpePeriodoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOpePeriodoResponse> Handle(CreateOpePeriodoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpePeriodoCommandHandler) + " - BEGIN");

        var opePeriodoEntity = _mapper.Map<OpePeriodo>(request);
        _unitOfWork.Repository<OpePeriodo>().AddEntity(opePeriodoEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope periodo");
        }

        _logger.LogInformation($"El ope periodo {opePeriodoEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpePeriodoCommandHandler) + " - END");
        return new CreateOpePeriodoResponse { Id = opePeriodoEntity.Id };
    }
}
