using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorIncendio;
public class GetRegistrosPorSucesoQueryHandler : IRequestHandler<GetRegistrosPorSucesoQuery, PaginationVm<RegistroActualizacionDto>>
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

    public async Task<PaginationVm<RegistroActualizacionDto>> Handle(GetRegistrosPorSucesoQuery request, CancellationToken cancellationToken)
    {
        var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);

        if (suceso == null || suceso.Borrado == true)
        {
            _logger.LogWarning($"No se encontro suceso con id: {request.IdSuceso}");
            throw new NotFoundException(nameof(Suceso), request.IdSuceso);
        }

        var specCounting = new RegistroActualizacionWithDetailsForCountingSpecification(new RegistroActualizacionSpecificationParams
        {
            IdSuceso = request.IdSuceso,
        });

        var totalRegistros = await _unitOfWork.Repository<RegistroActualizacion>().CountAsync(specCounting);
        if (totalRegistros == 0)
        {
            _logger.LogWarning($"No se encontraron registros para el suceso con id: {request.IdSuceso}");
            return new PaginationVm<RegistroActualizacionDto>
            {
                Count = totalRegistros,
                Data = new List<RegistroActualizacionDto>(),
                PageCount = 0,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        var spec = new RegistroActualizacionWithDetailsSpecification(new RegistroActualizacionSpecificationParams
        {
            IdSuceso = request.IdSuceso,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex
        });
        
        var registros = await _unitOfWork.Repository<RegistroActualizacion>().GetAllWithSpec(spec);

        // Obtener nombres de usuarios
        var guidsUsuarios = new HashSet<Guid?>();
        guidsUsuarios.UnionWith(registros.Select(r => r.CreadoPor));
        var nombresUsuarios = await ObtenerNombresUsuariosAsync(guidsUsuarios);

        var registrosDto = registros.Select(r => new RegistroActualizacionDto
        {
            Id = r.Id,
            FechaHora = r.FechaCreacion,
            TipoRegistro = new TipoRegistroDto
            {
                Id = r.TipoRegistroActualizacion.Id,
                Nombre = r.TipoRegistroActualizacion.Nombre
            },
            Apartados = string.Join(" / ", r.DetallesRegistro.Select(d => d.ApartadoRegistro.Nombre).Distinct().ToList()),
            Tecnico = nombresUsuarios.GetValueOrDefault(r.CreadoPor ?? Guid.Empty, "Desconocido"),
            EsUltimoRegistro = r.FechaCreacion == registros.Max(e => e.FechaCreacion)
        }).ToList();

        var rounded = Math.Ceiling(Convert.ToDecimal(totalRegistros) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var pagination = new PaginationVm<RegistroActualizacionDto>
        {
            Count = totalRegistros,
            Data = registrosDto,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };

        return pagination;
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

