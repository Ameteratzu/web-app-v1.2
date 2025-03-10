using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries;

public class GetIncendiosListQueryHandler : IRequestHandler<GetIncendiosListQuery, PaginationVm<IncendioVm>>
{
    private readonly ILogger<GetIncendiosListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetIncendiosListQueryHandler(
        ILogger<GetIncendiosListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationVm<IncendioVm>> Handle(GetIncendiosListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetIncendiosListQueryHandler)} - BEGIN");

        var spec = new IncendiosSpecification(request);
        var incendios = await _unitOfWork.Repository<Incendio>()
        .GetAllWithSpec(spec);

        var specCount = new IncendiosForCountingSpecification(request);
        var totalIncendios = await _unitOfWork.Repository<Incendio>().CountAsync(specCount);

        List<IncendioVm> incendioVmList = (await Task.WhenAll(incendios.Select(i => MapIncendioVmAsync(i, cancellationToken)))).ToList();

        var rounded = Math.Ceiling(Convert.ToDecimal(totalIncendios) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var pagination = new PaginationVm<IncendioVm>
        {
            Count = totalIncendios,
            Data = incendioVmList,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };

        _logger.LogInformation($"{nameof(GetIncendiosListQueryHandler)} - END");
        return pagination;
    }

    private async Task<IncendioVm> MapIncendioVmAsync(Incendio item, CancellationToken cancellationToken)
    {
        var incendioVm = _mapper.Map<IncendioVm>(item);
        incendioVm.Ubicacion = ObtenerUbicacion(item);

        RegistroActualizacion? ultimoRegistro = TryGetUltimoRegistro(item.Suceso.RegistroActualizaciones);

        (string? ultimaSituacionOperativa, string? maximaSituacionOperativa) = await ObtenerParametrosAsync(item);

        incendioVm.FechaUltimoRegistro = ultimoRegistro?.FechaCreacion;
        incendioVm.Sop = ultimaSituacionOperativa;
        incendioVm.MaxSop = maximaSituacionOperativa;

        return incendioVm;
    }

    private static string ObtenerUbicacion(Incendio item) =>
        item.IdTerritorio == (int)TipoTerritorio.Extranjero
            ? item.Pais.Descripcion
            : $"{item.Municipio.Descripcion} ({item.Provincia.Descripcion})";

    private RegistroActualizacion? TryGetUltimoRegistro(List<RegistroActualizacion> registros)
    {
        return registros?
            .OrderByDescending(r => r.FechaCreacion)
            .FirstOrDefault();
    }

    private Task<(string? UltimaSituacionOperativa, string? MaximaSituacionOperativa)> ObtenerParametrosAsync(Incendio item)
    {
        Evolucion? evolucion = item.Suceso?.Evolucion;
        if (evolucion == null) return Task.FromResult<(string?, string?)>((null, null));

        var parametros = evolucion.Parametros
            .Where(p => !p.Borrado && p.IdSituacionEquivalente != null)
            .ToList();

        var ultimaSituacionOperativa = parametros
            .OrderByDescending(p => p.FechaCreacion)
            .FirstOrDefault()?.SituacionEquivalente?.Descripcion;

        var maximaSituacionOperativa = parametros
            .OrderBy(p => p.SituacionEquivalente.Prioridad)
            .Select(p => p.SituacionEquivalente.Descripcion)
            .FirstOrDefault();

        return Task.FromResult((ultimaSituacionOperativa, maximaSituacionOperativa));
    }
}
