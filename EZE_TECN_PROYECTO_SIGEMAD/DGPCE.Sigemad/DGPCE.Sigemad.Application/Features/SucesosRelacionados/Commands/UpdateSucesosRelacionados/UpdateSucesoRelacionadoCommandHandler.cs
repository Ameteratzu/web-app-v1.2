using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.UpdateSucesosRelacionados;
public class UpdateSucesoRelacionadoCommandHandler : IRequestHandler<UpdateSucesoRelacionadoCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSucesoRelacionadoCommandHandler> _logger;

    public UpdateSucesoRelacionadoCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateSucesoRelacionadoCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateSucesoRelacionadoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateSucesoRelacionadoCommandHandler) + " - BEGIN");

        var sucesoPrincipal = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSucesoPrincipal);
        if (sucesoPrincipal == null)
        {
            _logger.LogWarning("Suceso principal no encontrado");
            throw new NotFoundException(nameof(Suceso), request.IdSucesoPrincipal);
        }

        var sucesoAsociado = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSucesoAsociado);
        if (sucesoAsociado == null)
        {
            _logger.LogWarning("Suceso asociado no encontrado");
            throw new NotFoundException(nameof(Suceso), request.IdSucesoAsociado);
        }

        var spec = new SucesoRelacionadoActiveByIdSpecification(request.Id);
        var sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(spec);

        if (sucesoRelacionado == null)
        {
            _logger.LogWarning($"{nameof(GetSucesoRelacionadoByIdQueryHandler)} - NOT FOUND");
            throw new NotFoundException(nameof(SucesoRelacionado), request.Id);
        }

        sucesoRelacionado.Observaciones = request.Observaciones;
        sucesoRelacionado.IdSucesoAsociado = request.IdSucesoAsociado;
        sucesoRelacionado.IdSucesoPrincipal = request.IdSucesoPrincipal;

        _unitOfWork.Repository<SucesoRelacionado>().UpdateEntity(sucesoRelacionado);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el suceso relacionado con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateSucesoRelacionadoCommandHandler) + " - END");

        return Unit.Value;
    }
}
