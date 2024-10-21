using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateImpactoEvoluciones;
public class UpdateImpactoEvolucionCommandHandler : IRequestHandler<UpdateImpactoEvolucionCommand>
{
    private readonly ILogger<UpdateImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateImpactoEvolucionCommandHandler(
        ILogger<UpdateImpactoEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateImpactoEvolucionCommand request, CancellationToken cancellationToken)
    {
        var impactoSpec = new ImpactoActiveByIdSpecification(request.Id);
        var impactoEvolucionToUpdate = await _unitOfWork.Repository<ImpactoEvolucion>().GetByIdWithSpec(impactoSpec);
        if (impactoEvolucionToUpdate == null)
        {
            _logger.LogWarning($"No se encontro impacto con id: {request.Id}");
            throw new NotFoundException(nameof(ImpactoEvolucion), request.Id);
        }

        var evolucionSpec = new EvolucionActiveByIdSpecification(request.IdEvolucion);
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);
        if (evolucion == null)
        {
            _logger.LogWarning($"No se encontro evolucion con id: {request.IdEvolucion}");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
        }

        _mapper.Map(request, impactoEvolucionToUpdate, typeof(UpdateImpactoEvolucionCommand), typeof(ImpactoEvolucion));

        _unitOfWork.Repository<ImpactoEvolucion>().UpdateEntity(impactoEvolucionToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente un impacto con id: {request.Id}");

        return Unit.Value;
    }
}
