using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Sucesos;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Features.Sucesos.Vms;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Sucesos;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetSucesosList;

public class GetSucesosListQueryHandler : IRequestHandler<GetSucesosListQuery, PaginationVm<SucesoGridDto>>
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

    public async Task<PaginationVm<SucesoGridDto>> Handle(GetSucesosListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetSucesosListQueryHandler)} - BEGIN");

        var spec = new SucesosSpecification(request);
        var sucesos = await _unitOfWork.Repository<Suceso>().GetAllWithSpec(spec);

        var specCount = new SucesoForCountingSpecification(request);
        var totalSucesos = await _unitOfWork.Repository<Suceso>().CountAsync(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalSucesos) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        //-------------------------------------
        var sucesosGrid = sucesos.Select(suceso =>
        {
            // Obtener la denominación desde la relación cargada
            string? estado = suceso.IdTipo switch
            {
                (int)TipoSucesoEnum.IncendioForestal => suceso.Incendios?.FirstOrDefault()?.EstadoSuceso?.Descripcion,
                _ => ""
            };

            string? denominacion = suceso.IdTipo switch
            {
                (int)TipoSucesoEnum.IncendioForestal => suceso.Incendios?.FirstOrDefault()?.Denominacion,
                _ => ""
            };

            return new SucesoGridDto
            {
                Id = suceso.Id,
                FechaCreacion = suceso.FechaCreacion,
                FechaModificacion = suceso.FechaModificacion,
                TipoSuceso = suceso.TipoSuceso.Descripcion,
                Estado = estado ?? "",
                Denominacion = denominacion ?? "",
            };

        }).ToList();


        //-------------------------------------

        var pagination = new PaginationVm<SucesoGridDto>
        {
            Count = totalSucesos,
            Data = sucesosGrid,
            PageCount = totalPages,
            Page = request.Page,
            PageSize = request.PageSize
        };


        _logger.LogInformation($"{nameof(GetSucesosListQueryHandler)} - END");
        return pagination;
    }
}