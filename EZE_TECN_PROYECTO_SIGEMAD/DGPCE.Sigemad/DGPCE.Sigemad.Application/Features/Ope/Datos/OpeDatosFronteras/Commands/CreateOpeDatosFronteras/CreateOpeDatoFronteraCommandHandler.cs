using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class CreateOpeDatoFronteraCommandHandler : IRequestHandler<CreateOpeDatoFronteraCommand, CreateOpeDatoFronteraResponse>
{
    private readonly ILogger<CreateOpeDatoFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOpeDatoFronteraCommandHandler(
        ILogger<CreateOpeDatoFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOpeDatoFronteraResponse> Handle(CreateOpeDatoFronteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeDatoFronteraCommandHandler) + " - BEGIN");

        var opeDatoFronteraEntity = _mapper.Map<OpeDatoFrontera>(request);
        _unitOfWork.Repository<OpeDatoFrontera>().AddEntity(opeDatoFronteraEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope dato de frontera");
        }

        _logger.LogInformation($"El ope dato de frontera {opeDatoFronteraEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpeDatoFronteraCommandHandler) + " - END");
        return new CreateOpeDatoFronteraResponse { Id = opeDatoFronteraEntity.Id };
    }
}
