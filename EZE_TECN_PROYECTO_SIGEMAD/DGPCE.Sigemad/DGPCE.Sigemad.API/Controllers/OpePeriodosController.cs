using Azure.Core;
using DGPCE.Sigemad.Application.Features.OpePeriodos.Commands.CreateOpePeriodos;
using DGPCE.Sigemad.Application.Features.OpePeriodos.Commands.DeleteOpePeriodos;
using DGPCE.Sigemad.Application.Features.OpePeriodos.Commands.UpdateOpePeriodos;
using DGPCE.Sigemad.Application.Features.Periodos.Queries.GetPeriodoById;
using DGPCE.Sigemad.Application.Features.Periodos.Queries.GetPeriodosList;
using DGPCE.Sigemad.Application.Features.Periodos.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/ope-periodos")]
[ApiController]
public class OpePeriodosController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpePeriodosController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpePeriodo")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpePeriodoResponse>> Create([FromBody] CreateOpePeriodoCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
 
    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpePeriodo>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpePeriodoVm>>> GetOpePeriodos(
        [FromQuery] GetOpePeriodosListQuery query)
    {
        Console.WriteLine("2222222222222222222222222: " + query.Search);
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de periodo de OPE por id")]
    public async Task<ActionResult<OpePeriodo>> GetById(int id)
    {
        var query = new GetOpePeriodoByIdQuery(id);
        var periodo = await _mediator.Send(query);

        if (periodo == null)
            return NotFound();

        return Ok(periodo);
    }

    [HttpPut(Name = "UpdateOpePeriodo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpePeriodoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpePeriodo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpePeriodoCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
