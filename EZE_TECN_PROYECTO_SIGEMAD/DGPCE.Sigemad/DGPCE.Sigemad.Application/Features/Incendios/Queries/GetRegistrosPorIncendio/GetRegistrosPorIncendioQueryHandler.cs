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

        // Obtener listado de usuarios
        var guidsUsuarios = new HashSet<Guid?>();

        guidsUsuarios.UnionWith(incendio.Evoluciones.Select(d => d.CreadoPor));
        guidsUsuarios.UnionWith(incendio.DireccionCoordinacionEmergencias.Select(d => d.CreadoPor));
        guidsUsuarios.UnionWith(incendio.OtraInformaciones.Select(o => o.CreadoPor));
        guidsUsuarios.UnionWith(incendio.Documentaciones.Select(d => d.CreadoPor));

        // Obtener nombres de usuarios
        var nombresUsuarios = await ObtenerNombresUsuariosAsync(guidsUsuarios);

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
            Tecnico = nombresUsuarios.TryGetValue(d.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = d.FechaCreacion == incendio.Evoluciones.Max(e => e.FechaCreacion)
        }));

        // Procesar Otra Información
        registros.AddRange(incendio.OtraInformaciones.Select(o => new RegistroActualizacionDto
        {
            Id = o.Id,
            FechaHora = o.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Otra Información",
            Tecnico = nombresUsuarios.TryGetValue(o.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = o.FechaCreacion == incendio.OtraInformaciones.Max(e => e.FechaCreacion)
        }));

        // Procesar Direcciones y Coordinación
        registros.AddRange(incendio.DireccionCoordinacionEmergencias.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Dirección y coordinación",
            Tecnico = nombresUsuarios.TryGetValue(d.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = d.FechaCreacion == incendio.DireccionCoordinacionEmergencias.Max(e => e.FechaCreacion)
        }));

        // Procesar Documentacion
        registros.AddRange(incendio.Documentaciones.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Documentación",
            Tecnico = nombresUsuarios.TryGetValue(d.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = d.FechaCreacion == incendio.Documentaciones.Max(e => e.FechaCreacion)
        }));

        // Ordenar por FechaHora descendente
        return registros.OrderByDescending(r => r.FechaHora).ToList();
    }

    /// <summary>
    /// Método para obtener los nombres de los usuarios a partir de sus GUIDs.
    /// </summary>
    private async Task<Dictionary<Guid, string>> ObtenerNombresUsuariosAsync(IEnumerable<Guid?> guidsUsuarios)
    {
        // 1. Filtrar valores nulos y duplicados
        var guidsFiltrados = guidsUsuarios.Where(g => g.HasValue).Select(g => g.Value).Distinct().ToList();

        if (!guidsFiltrados.Any())
        {
            return new Dictionary<Guid, string>();
        }

        // 2. Consultar la tabla ApplicationUser
        var usuarios = await _unitOfWork.Repository<ApplicationUser>()
            .GetAsync(u => guidsFiltrados.Contains(u.Id));

        // 3. Crear un diccionario para facilitar el acceso a los nombres
        return usuarios.ToDictionary(
            u => u.Id, // Clave: GUID del usuario
            u => u.Nombre ?? "Desconocido" // Valor: Nombre del usuario
        );
    }


}

