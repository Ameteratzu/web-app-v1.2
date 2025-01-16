using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Features.CoordinacionCecopis.Commands.CreateCoordinacionCecopi;
using DGPCE.Sigemad.Application.Features.CoordinacionesPma.Commands.CreateOrUpdateCoordinacionPma;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Delete;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.DireccionCoordinacionEmergenciasById;
using DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/direcciones-coordinaciones-emergencias")]
[ApiController]
public class DireccionCoordinacionEmergenciasController : ControllerBase
{
    private readonly IMediator _mediator;

    public DireccionCoordinacionEmergenciasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id:int}", Name = "DeleteDireccionCoordinacionEmergencia")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteDireccionCoordinacionEmergenciaCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtener DireccionCoordinacionEmergencia mediante id")]
    public async Task<ActionResult<DireccionCoordinacionEmergencia>> GetDireccionCoordinacionEmergencianById(int id)
    {
        var query = new GetDireccionCoordinacionEmergenciasById(id);
        var impacto = await _mediator.Send(query);
        return Ok(impacto);
    }

    [HttpPost("direcciones")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateDireccionResponse>> CreateDirecciones([FromBody] CreateOrUpdateDireccionCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("coordinaciones-cecopi")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateCoordinacionCecopiResponse>> CreateCoordinacionesCecopi([FromBody] CreateOrUpdateCoordinacionCecopiCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("coordinaciones-pma")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateCoordinacionPmaResponse>> CreateCoordinacionesPma([FromBody] CreateOrUpdateCoordinacionPmaCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
