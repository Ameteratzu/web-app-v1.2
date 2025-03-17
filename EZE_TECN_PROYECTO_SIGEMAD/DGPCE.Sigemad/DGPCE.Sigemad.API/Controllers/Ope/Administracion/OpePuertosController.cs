using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.DeleteOpePuertos;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Queries.GetOpePuertoById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.CreateOpePuertos;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.UpdateOpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using GetOpePuertosListQuery = DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Queries.GetOpePuertosList.GetOpePuertosListQuery;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-puertos")]
[ApiController]
public class OpePuertosController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpePuertosController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpePuerto")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpePuertoResponse>> Create([FromBody] CreateOpePuertoCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpePuerto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpePuertoVm>>> GetOpePuertos(
        [FromQuery] GetOpePuertosListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de puerto de OPE por id")]
    public async Task<ActionResult<OpePuerto>> GetById(int id)
    {
        var query = new GetOpePuertoByIdQuery(id);
        var opePuerto = await _mediator.Send(query);

        if (opePuerto == null)
            return NotFound();

        return Ok(opePuerto);
    }

    [HttpPut(Name = "UpdateOpePuerto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpePuertoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpePuerto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpePuertoCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
