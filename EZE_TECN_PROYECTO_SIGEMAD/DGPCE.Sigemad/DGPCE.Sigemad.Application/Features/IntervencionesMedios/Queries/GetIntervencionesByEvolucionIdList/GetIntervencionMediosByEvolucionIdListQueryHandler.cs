using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.Intervenciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Queries.GetIntervencionesByEvolucionIdList;
public class GetIntervencionMediosByEvolucionIdListQueryHandler : IRequestHandler<GetIntervencionMediosByEvolucionIdListQuery, IReadOnlyList<IntervencionMedio>>
{
    private readonly ILogger<GetIntervencionMediosByEvolucionIdListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetIntervencionMediosByEvolucionIdListQueryHandler(
        ILogger<GetIntervencionMediosByEvolucionIdListQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<IntervencionMedio>> Handle(GetIntervencionMediosByEvolucionIdListQuery request, CancellationToken cancellationToken)
    {
        var evolucionSpec = new EvolucionActiveByIdSpecification(request.IdEvolucion);
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);
        if (evolucion == null || evolucion.Borrado == true)
        {
            _logger.LogWarning($"No se encontro evolucion con id: {request.IdEvolucion}");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
        }

        var intervencionSpec = new IntervencionActiveByIdEvolucionSpecification(request.IdEvolucion);
        var intervenciones = await _unitOfWork.Repository<IntervencionMedio>().GetAllWithSpec(intervencionSpec);
        return intervenciones;
    }
}
