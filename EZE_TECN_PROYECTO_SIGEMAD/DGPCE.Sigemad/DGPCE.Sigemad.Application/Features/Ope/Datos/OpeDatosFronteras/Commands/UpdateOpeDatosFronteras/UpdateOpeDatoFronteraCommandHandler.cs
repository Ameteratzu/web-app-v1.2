using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeDatosFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.UpdateOpeDatosFronteras;

public class UpdateOpeDatoFronteraCommandHandler : IRequestHandler<UpdateOpeDatoFronteraCommand>
{
    private readonly ILogger<UpdateOpeDatoFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOpeDatoFronteraCommandHandler(
        ILogger<UpdateOpeDatoFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOpeDatoFronteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeDatoFronteraCommandHandler) + " - BEGIN");

        var opeDatoFronteraSpec = new OpeDatoFronteraActiveByIdSpecification(request.Id);
        var opeDatoFronteraToUpdate = await _unitOfWork.Repository<OpeDatoFrontera>().GetByIdWithSpec(opeDatoFronteraSpec);


        if (opeDatoFronteraToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope dato de frontera con id: {request.Id}");
            throw new NotFoundException(nameof(OpeDatoFrontera), request.Id);
        }

        _mapper.Map(request, opeDatoFronteraToUpdate, typeof(UpdateOpeDatoFronteraCommand), typeof(OpeDatoFrontera));

        _unitOfWork.Repository<OpeDatoFrontera>().UpdateEntity(opeDatoFronteraToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope dato de frontera con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeDatoFronteraCommandHandler) + " - END");

        return Unit.Value;
    }
}
