
using DGPCE.Sigemad.Application.Features.Shared;
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

}
