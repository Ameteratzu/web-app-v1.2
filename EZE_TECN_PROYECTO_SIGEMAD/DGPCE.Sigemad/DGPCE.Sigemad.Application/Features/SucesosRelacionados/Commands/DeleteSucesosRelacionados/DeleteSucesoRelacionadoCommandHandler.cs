using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesosRelacionados;
public class DeleteSucesoRelacionadoCommandHandler : IRequestHandler<DeleteSucesoRelacionadoCommand>
{
    public readonly IUnitOfWork _unitOfWork;
    public readonly ILogger<DeleteSucesoRelacionadoCommandHandler> _logger;

    public DeleteSucesoRelacionadoCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteSucesoRelacionadoCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteSucesoRelacionadoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(DeleteSucesoRelacionadoCommand) + " - BEGIN");

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

        var spec = new SucesoRelacionadoActiveByIdPrincipalAndIdAsociadoSpecification(request.IdSucesoPrincipal, request.IdSucesoAsociado);
        var sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(spec);

        if (sucesoRelacionado == null)
        {
            _logger.LogWarning($"{nameof(DeleteSucesoRelacionadoCommandHandler)} - NOT FOUND");
            throw new NotFoundException(nameof(SucesoRelacionado), request.IdSucesoPrincipal);
        }

        sucesoRelacionado.Borrado = true;
        _unitOfWork.Repository<SucesoRelacionado>().UpdateEntity(sucesoRelacionado);
        var result = await _unitOfWork.Complete();

        if (result == 0)
        {
            _logger.LogError("No se pudo eliminar el suceso relacionado");
            throw new Exception("No se pudo eliminar el suceso relacionado");
        }

        _logger.LogInformation($"SucesoRelacionado con Id {sucesoRelacionado.Id} eliminado lógico correcto");

        return Unit.Value;
    }
}
