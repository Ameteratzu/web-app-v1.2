using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
public class CreateImpactoEvolucionCommandHandler : IRequestHandler<CreateImpactoEvolucionCommand, CreateImpactoEvolucionResponse>
{
    private readonly ILogger<CreateImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateImpactoEvolucionCommandHandler(
        ILogger<CreateImpactoEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateImpactoEvolucionResponse> Handle(CreateImpactoEvolucionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateImpactoEvolucionCommandHandler)} - BEGIN");

        var evolucionSpec = new EvolucionActiveByIdSpecification(request.IdEvolucion);
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);
        if (evolucion is null)
        {
            _logger.LogWarning($"request.IdEvolucion: {request.IdEvolucion}, no encontrado");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
        }

        var impactoClasificado = await _unitOfWork.Repository<ImpactoClasificado>().GetByIdAsync(request.IdImpactoClasificado);
        if (impactoClasificado is null)
        {
            _logger.LogWarning($"request.IdImpactoClasificado: {request.IdImpactoClasificado}, no encontrado");
            throw new NotFoundException(nameof(ImpactoClasificado), request.IdImpactoClasificado);
        }


        var impactoEvolucionEntity = _mapper.Map<ImpactoEvolucion>(request);
        _unitOfWork.Repository<ImpactoEvolucion>().AddEntity(impactoEvolucionEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo impacto evolucion");
        }
        _logger.LogInformation($"El impacto evolucion {impactoEvolucionEntity.Id} fue creado correctamente");

        _logger.LogInformation($"{nameof(CreateImpactoEvolucionCommandHandler)} - END");
        return new CreateImpactoEvolucionResponse { Id = impactoEvolucionEntity.Id };
    }
}
