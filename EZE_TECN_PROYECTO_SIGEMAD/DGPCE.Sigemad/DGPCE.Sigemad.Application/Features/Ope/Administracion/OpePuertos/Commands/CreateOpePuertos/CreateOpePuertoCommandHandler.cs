using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Commands.CreateOpePuertos;

public class CreateOpePuertoCommandHandler : IRequestHandler<CreateOpePuertoCommand, CreateOpePuertoResponse>
{
    private readonly ILogger<CreateOpePuertoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOpePuertoCommandHandler(
        ILogger<CreateOpePuertoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOpePuertoResponse> Handle(CreateOpePuertoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpePuertoCommandHandler) + " - BEGIN");

        var opePuertoEntity = _mapper.Map<OpePuerto>(request);
        _unitOfWork.Repository<OpePuerto>().AddEntity(opePuertoEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope puerto");
        }

        _logger.LogInformation($"El ope puerto {opePuertoEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpePuertoCommandHandler) + " - END");
        return new CreateOpePuertoResponse { Id = opePuertoEntity.Id };
    }
}
