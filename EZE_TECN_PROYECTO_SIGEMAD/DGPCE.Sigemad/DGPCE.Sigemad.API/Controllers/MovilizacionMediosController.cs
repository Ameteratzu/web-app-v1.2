using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Features.MovilizacionMedios.Queries.GetTipoGestion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/movilizaciones-medios")]
[ApiController]
public class MovilizacionMediosController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovilizacionMediosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("tipos-gestion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene los tipos de gestion")]
    public async Task<ActionResult<IReadOnlyList<TipoGestionDto>>> GetTipoGestion([FromQuery] GetTipoGestionQuery query)
    {
        var tipoGestiones = await _mediator.Send(query);
        return Ok(tipoGestiones);
    }
}
