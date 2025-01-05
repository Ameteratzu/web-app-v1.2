
using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorIncendio;
using DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetSucesosList;
using DGPCE.Sigemad.Application.Features.Sucesos.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class SucesosController : ControllerBase
{
    private readonly IMediator _mediator;

    public SucesosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<SucesosBusquedaVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<SucesosBusquedaVm>>> GetSucesos(
        [FromQuery] GetSucesosListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }

    [HttpGet("{id}/registros")]
    [ProducesResponseType(typeof(IReadOnlyList<RegistroActualizacionDto>), (int)HttpStatusCode.OK)]
    public async Task<IReadOnlyList<RegistroActualizacionDto>> GetIncendioDetalles(int id)
    {
        var query = new GetRegistrosPorSucesoQuery(id);
        var result = await _mediator.Send(query);
        return result;
    }

}
