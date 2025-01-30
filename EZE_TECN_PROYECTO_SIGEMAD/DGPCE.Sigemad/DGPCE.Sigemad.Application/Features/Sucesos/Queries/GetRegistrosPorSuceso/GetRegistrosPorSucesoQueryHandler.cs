using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Sucesos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks.Dataflow;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorIncendio;
public class GetRegistrosPorSucesoQueryHandler : IRequestHandler<GetRegistrosPorSucesoQuery, IReadOnlyList<RegistroActualizacionDto>>
{
    private readonly ILogger<GetRegistrosPorSucesoQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetRegistrosPorSucesoQueryHandler(
        ILogger<GetRegistrosPorSucesoQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<RegistroActualizacionDto>> Handle(GetRegistrosPorSucesoQuery request, CancellationToken cancellationToken)
    {
        // Usar la especificación para obtener el suceso con todos los registros relacionados
        var suceso = await _unitOfWork.Repository<Suceso>()
            .GetByIdWithSpec(new SucesoWithAllRegistrosSpecification(request.IdSuceso));

        if (suceso == null)
        {
            _logger.LogWarning($"No se encontro suceso con id: {request.IdSuceso}");
            throw new NotFoundException(nameof(Suceso), request.IdSuceso);
        }

        // Obtener listado de usuarios
        var guidsUsuarios = new HashSet<Guid?>();

        guidsUsuarios.UnionWith(suceso.Evoluciones.Select(d => d.CreadoPor));
        guidsUsuarios.UnionWith(suceso.DireccionCoordinacionEmergencias.Select(d => d.CreadoPor));
        guidsUsuarios.UnionWith(suceso.OtraInformaciones.Select(o => o.CreadoPor));
        guidsUsuarios.UnionWith(suceso.Documentaciones.Select(d => d.CreadoPor));
        guidsUsuarios.UnionWith(suceso.SucesoRelacionados.Select(d => d.CreadoPor));
        guidsUsuarios.UnionWith(suceso.ActuacionesRelevantes.Select(d => d.CreadoPor));

        // Obtener nombres de usuarios
        var nombresUsuarios = await ObtenerNombresUsuariosAsync(guidsUsuarios);

        // Crear el listado consolidado
        var registros = new List<RegistroActualizacionDto>();

        // Procesar Datos de Evolución
        registros.AddRange(suceso.Evoluciones.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Datos de evolución",
            Apartados = string.Join(" / ", GetTitulosDeApartados(d)),
            Tecnico = nombresUsuarios.TryGetValue(d.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = d.FechaCreacion == suceso.Evoluciones.Max(e => e.FechaCreacion)
        }));

        // Procesar Otra Información
        registros.AddRange(suceso.OtraInformaciones.Select(o => new RegistroActualizacionDto
        {
            Id = o.Id,
            FechaHora = o.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Otra Información",
            Apartados = string.Join(" / ", GetTitulosDeApartados(o)),
            Tecnico = nombresUsuarios.TryGetValue(o.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = o.FechaCreacion == suceso.OtraInformaciones.Max(e => e.FechaCreacion)
        }));

        // Procesar Direcciones y Coordinación
        registros.AddRange(suceso.DireccionCoordinacionEmergencias.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Dirección y coordinación",
            Apartados = string.Join(" / ", GetTitulosDeApartados(d)),
            Tecnico = nombresUsuarios.TryGetValue(d.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = d.FechaCreacion == suceso.DireccionCoordinacionEmergencias.Max(e => e.FechaCreacion)
        }));

        // Procesar Documentacion
        registros.AddRange(suceso.Documentaciones.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Documentación",
            Apartados = string.Join(" / ", GetTitulosDeApartados(d)),
            Tecnico = nombresUsuarios.TryGetValue(d.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = d.FechaCreacion == suceso.Documentaciones.Max(e => e.FechaCreacion)
        }));

        // Procesar Sucesos Relacionados
        registros.AddRange(suceso.SucesoRelacionados.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Sucesos Relacionados",
            Apartados = string.Join(" / ", GetTitulosDeApartados(d)),
            Tecnico = nombresUsuarios.TryGetValue(d.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = d.FechaCreacion == suceso.SucesoRelacionados.Max(e => e.FechaCreacion)
        }));

        // Procesar Actuaciones Relevantes
        registros.AddRange(suceso.ActuacionesRelevantes.Select(d => new RegistroActualizacionDto
        {
            Id = d.Id,
            FechaHora = d.FechaCreacion,
            Registro = "",
            Origen = "",
            TipoRegistro = "Actuaciones Relevantes",
            Apartados = string.Join(" / ", GetTitulosDeApartados(d)),
            Tecnico = nombresUsuarios.TryGetValue(d.CreadoPor ?? Guid.Empty, out var nombre) ? nombre : "Desconocido",
            EsUltimoRegistro = d.FechaCreacion == suceso.ActuacionesRelevantes.Max(e => e.FechaCreacion)
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

    private static List<string> GetTitulosDeApartados(Evolucion evolucion)
    {
        var titulos = new List<string>();

        // Verifica si "Registro" tiene datos
        if (evolucion.Registro != null)
        {
            titulos.Add("Registro");
        }

        // Verifica si "DatoPrincipal" tiene datos
        if (evolucion.DatoPrincipal != null)
        {
            titulos.Add("Dato Principal");
        }

        // Verifica si "Parametro" tiene datos
        if (evolucion.Parametro != null)
        {
            titulos.Add("Parámetro");
        }

        // Verifica si "AreaAfectadas" tiene al menos un elemento
        if (evolucion.AreaAfectadas != null && evolucion.AreaAfectadas.Any())
        {
            titulos.Add("Area Afectadas");
        }

        // Verifica si "Impactos" tiene al menos un elemento
        if (evolucion.Impactos != null && evolucion.Impactos.Any())
        {
            titulos.Add("Impactos");
        }

        return titulos;
    }

    private static List<string> GetTitulosDeApartados(DireccionCoordinacionEmergencia direccionCoordinacionEmergencia)
    {
        var titulos = new List<string>();

        if (direccionCoordinacionEmergencia.Direcciones != null && direccionCoordinacionEmergencia.Direcciones.Any())
        {
            titulos.Add("Dirección");
        }

        if (direccionCoordinacionEmergencia.CoordinacionesCecopi != null && direccionCoordinacionEmergencia.CoordinacionesCecopi.Any())
        {
            titulos.Add("Coordinación CECOPI");
        }

        if (direccionCoordinacionEmergencia.CoordinacionesPMA != null && direccionCoordinacionEmergencia.CoordinacionesPMA.Any())
        {
            titulos.Add("Coordinación PMA");
        }

        return titulos;
    }

    private static List<string> GetTitulosDeApartados(Documentacion documentacion)
    {
        var titulos = new List<string>();

        if (documentacion.DetallesDocumentacion != null && documentacion.DetallesDocumentacion.Any())
        {
            titulos.Add("Documentación");
        }

        return titulos;
    }

    private static List<string> GetTitulosDeApartados(OtraInformacion otraInformacion)
    {
        var titulos = new List<string>();

        if (otraInformacion.DetallesOtraInformacion != null && otraInformacion.DetallesOtraInformacion.Any())
        {
            titulos.Add("Otra Información");
        }

        return titulos;
    }

    private static List<string> GetTitulosDeApartados(SucesoRelacionado sucesoRelacionado)
    {
        var titulos = new List<string>();

        if (sucesoRelacionado.DetalleSucesoRelacionados != null && sucesoRelacionado.DetalleSucesoRelacionados.Any())
        {
            titulos.Add("Suceso Relacionado");
        }

        return titulos;
    }

    private static List<string> GetTitulosDeApartados(ActuacionRelevanteDGPCE actuacionRelevante)
    {
        var titulos = new List<string>();

        if (actuacionRelevante.EmergenciaNacional != null)
        {
            titulos.Add("Emergencia Nacional");
        }

        if (actuacionRelevante.ActivacionPlanEmergencias != null && actuacionRelevante.ActivacionPlanEmergencias.Any())
        {
            titulos.Add("Activacion Plan Emergencias");
        }

        if (actuacionRelevante.DeclaracionesZAGEP != null && actuacionRelevante.DeclaracionesZAGEP.Any())
        {
            titulos.Add("Declaraciones ZAGEP");
        }

        if (actuacionRelevante.ActivacionSistemas != null && actuacionRelevante.ActivacionSistemas.Any())
        {
            titulos.Add("Activacion Sistemas");
        }

        if (actuacionRelevante.ConvocatoriasCECOD != null && actuacionRelevante.ConvocatoriasCECOD.Any())
        {
            titulos.Add("Convocatorias CECOD");
        }

        if (actuacionRelevante.NotificacionesEmergencias != null && actuacionRelevante.NotificacionesEmergencias.Any())
        {
            titulos.Add("Notificaciones Emergencias");
        }
      

        return titulos;
    }



}

