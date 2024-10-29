using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendioDetalles;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendioDetallesList;
public class GetIncendioDetallesListQueryHandler : IRequestHandler<GetIncendioDetallesListQuery, IReadOnlyList<IncendioDetalleVm>>
{
    private readonly ILogger<GetIncendioDetallesListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetIncendioDetallesListQueryHandler(
        ILogger<GetIncendioDetallesListQueryHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<IncendioDetalleVm>> Handle(GetIncendioDetallesListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetIncendioDetallesListQueryHandler)} - BEGIN");

        var evolucionSpec = new DetalleEvolucionSpecification(request.IdIncendio);
        IReadOnlyList<Evolucion> detallesEvolucion = await _unitOfWork.Repository<Evolucion>()
            .GetAllWithSpec(evolucionSpec) ?? new List<Evolucion>();

        var detallesAll = detallesEvolucion
            .Select(d => new IncendioDetalleVm
            {
                FechaRegistro = d.FechaCreacion,
                Registro = d.EntradaSalida.Descripcion,
                Origen = "",
                TipoRegistro = "Datos de evolución",
            })
            .OrderByDescending(d => d.FechaRegistro)
            .ToList();

        _logger.LogInformation($"{nameof(GetIncendioDetallesListQueryHandler)} - END");

        return detallesAll;
    }
}
