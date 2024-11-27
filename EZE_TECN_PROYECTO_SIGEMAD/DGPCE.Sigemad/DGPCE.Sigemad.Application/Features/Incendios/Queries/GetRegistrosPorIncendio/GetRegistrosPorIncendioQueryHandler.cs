using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetRegistrosDeIncendio;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetRegistrosPorIncendio;
public class GetRegistrosPorIncendioQueryHandler : IRequestHandler<GetRegistrosPorIncendioQuery, IReadOnlyList<RegistroActualizacionDto>>
{
    private readonly ILogger<GetRegistrosPorIncendioQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetRegistrosPorIncendioQueryHandler(
        ILogger<GetRegistrosPorIncendioQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<RegistroActualizacionDto>> Handle(GetRegistrosPorIncendioQuery request, CancellationToken cancellationToken)
    {
        // Usar la especificación para obtener el incendio con todos los registros relacionados
        var incendio = await _unitOfWork.Repository<Incendio>()
            .GetByIdWithSpec(new IncendioWithAllRegistrosSpecification(request.IdIncendio));

        if (incendio == null)
        {
            _logger.LogWarning($"No se encontro incendio con id: {request.IdIncendio}");
            throw new NotFoundException(nameof(Incendio), request.IdIncendio);
        }

        // Crear el listado consolidado
        var registros = new List<RegistroActualizacionDto>();

        // Procesar Datos de Evolución
        registros.AddRange(incendio.Evoluciones.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Datos de evolución",
            Tecnico = ""
        }));

        // Procesar Otra Información
        registros.AddRange(incendio.OtraInformaciones.Select(o => new RegistroActualizacionDto
        {
            Id = o.Id,
            FechaHora = o.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Otra Información",
            Tecnico = ""
        }));

        // Procesar Direcciones y Coordinación
        registros.AddRange(incendio.DireccionCoordinacionEmergencias.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Dirección y coordinación",
            Tecnico = ""
        }));

        // Ordenar por FechaHora descendente
        return registros.OrderByDescending(r => r.FechaHora).ToList();
    }
}

