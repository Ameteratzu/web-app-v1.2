using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Features.Sucesos.Vms;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetSucesosList;

public class GetSucesosListQueryHandler : IRequestHandler<GetSucesosListQuery, PaginationVm<SucesosBusquedaVm>>
{
    private readonly ILogger<GetSucesosListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSucesosListQueryHandler(
        ILogger<GetSucesosListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationVm<SucesosBusquedaVm>> Handle(GetSucesosListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetSucesosListQueryHandler)} - BEGIN");

        //Versión inicial solo con incendios ya que no existen mas sucesos en la aplicacion
        var incendiosParams = _mapper.Map<IncendiosSpecificationParams>(request);
        incendiosParams.busquedaSucesos = true;
        var spec = new IncendiosSpecification(incendiosParams);
        var incendios = await _unitOfWork.Repository<Incendio>()
        .GetAllWithSpec(spec);

       var sucesosVm = _mapper.Map<IReadOnlyList<SucesosBusquedaVm>>(incendios);


        var specCount = new IncendiosForCountingSpecification(incendiosParams);
        var totalSucesos = await _unitOfWork.Repository<Incendio>().CountAsync(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalSucesos) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var pagination = new PaginationVm<SucesosBusquedaVm>
        {
            Count = totalSucesos,
            Data = sucesosVm,
            PageCount = totalPages,
            Page = request.Page,
            PageSize = request.PageSize
        };

        _logger.LogInformation($"{nameof(GetSucesosListQueryHandler)} - END");
        return pagination;
    }
}