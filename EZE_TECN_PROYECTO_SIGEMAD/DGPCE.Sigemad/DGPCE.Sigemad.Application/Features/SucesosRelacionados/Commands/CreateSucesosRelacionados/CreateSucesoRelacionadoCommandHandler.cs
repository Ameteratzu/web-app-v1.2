using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.CreateSucesosRelacionados;
public class CreateSucesoRelacionadoCommandHandler : IRequestHandler<CreateSucesoRelacionadoCommand, CreateSucesoRelacionadoResponse>
{
    private readonly ILogger<CreateSucesoRelacionadoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSucesoRelacionadoCommandHandler(
        ILogger<CreateSucesoRelacionadoCommandHandler> logger, 
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateSucesoRelacionadoResponse> Handle(CreateSucesoRelacionadoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateSucesoRelacionadoCommand) + " - BEGIN");

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

        var sucesoRelacionado = _mapper.Map<SucesoRelacionado>(request);

        _unitOfWork.Repository<SucesoRelacionado>().AddEntity(sucesoRelacionado);
        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo suceso relacionado");
        }

        _logger.LogInformation($"El suceso relacionado {sucesoRelacionado.Id} fue creado correctamente");
        _logger.LogInformation(nameof(CreateSucesoRelacionadoCommand) + " - END");
        return new CreateSucesoRelacionadoResponse { Id = sucesoRelacionado.Id };
    }
}
