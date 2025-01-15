using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergencias;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/planes-emergencias")]
[ApiController]
public class PlanesEmergenciasController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlanesEmergenciasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de planes de emergencia completas o filtradas")]
    public async Task<ActionResult<IReadOnlyList<PlanEmergenciaVm>>> GetPlanesEmergencias([FromQuery] GetPlanesEmergenciasQuery query)
    {
        var planesEmergencias = await _mediator.Send(query);
        return Ok(planesEmergencias);
    }

}
