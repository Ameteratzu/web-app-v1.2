using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorIncendio;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
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
    private readonly GetRegistrosPorSucesoQueryHandler _getRegistrosSucesos;

    public GetIncendiosListQueryHandler(
        ILogger<GetIncendiosListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        GetRegistrosPorSucesoQueryHandler getRegistrosSucesos,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _getRegistrosSucesos = getRegistrosSucesos;
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

        var incendioVmList = new List<IncendioVm>();

        foreach (var item in incendios)
        {
            var incencioVm = new IncendioVm();

            if (item.IdTerritorio == (int)TipoTerritorio.Extranjero)
            {
                item.Ubicacion = $"{item.Pais.Descripcion}";
            }
            else
            {
                item.Ubicacion = $"{item.Municipio.Descripcion} ({item.Provincia.Descripcion})";
            }

            _mapper.Map(item, incencioVm);

            var registros = await _getRegistrosSucesos.Handle(new GetRegistrosPorSucesoQuery { IdSuceso = item.IdSuceso,PageSize = 1000}, cancellationToken);

            if (registros.Data != null && registros.Data.Count > 0)
            {
                var ultimoRegistro = registros.Data
                     .Where(r => r.EsUltimoRegistro)
                     .OrderByDescending(r => r.FechaHora)
                     .FirstOrDefault();

                incencioVm.FechaUltimoRegistro = ultimoRegistro != null ? ultimoRegistro.FechaHora : null;
            }

            if (item.Suceso.Evoluciones != null) { 

                string? maxSop;
                List<Evolucion> evoluciones;

                evoluciones = item.Suceso.Evoluciones;

                var evolucionItem = evoluciones
                             .OrderByDescending(e => e.FechaCreacion)
                             .FirstOrDefault();

                if (evolucionItem != null && evolucionItem.Parametro != null && !evolucionItem.Parametro.Borrado && evolucionItem.Parametro.IdSituacionEquivalente != null)
                {
                    incencioVm.Sop = evolucionItem
                                         .Parametro
                                         .SituacionEquivalente
                                         .Descripcion;
                }

                maxSop = evoluciones
                             .Where(e => !e.Borrado && e.Parametro != null && !e.Parametro.Borrado && e.Parametro.SituacionEquivalente != null)
                             .OrderBy(e => e.Parametro.SituacionEquivalente.Prioridad)
                             .Select(e => e.Parametro.SituacionEquivalente.Descripcion)
                             .FirstOrDefault();

                incencioVm.MaxSop = maxSop;

            }
        
            incendioVmList.Add(incencioVm);
        }



        var rounded = Math.Ceiling(Convert.ToDecimal(totalIncendios) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var pagination = new PaginationVm<IncendioVm>
        {
            Count = totalIncendios,
            Data = incendioVmList,
            PageCount = totalPages,
            Page = request.Page,
            PageSize = request.PageSize
        };

        _logger.LogInformation($"{nameof(GetIncendiosListQueryHandler)} - END");
        return pagination;
    }
}
