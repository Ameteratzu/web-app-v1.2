using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.DeleteOpeFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Queries.GetOpeFronteraById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.CreateOpeFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.UpdateOpeFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Queries.GetOpeFronterasList;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-fronteras")]
[ApiController]
public class OpeFronterasController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeFronterasController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpeFrontera")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeFronteraResponse>> Create([FromBody] CreateOpeFronteraCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeFrontera>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeFronteraVm>>> GetOpeFronteras(
        [FromQuery] GetOpeFronterasListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de frontera de OPE por id")]
    public async Task<ActionResult<OpeFrontera>> GetById(int id)
    {
        var query = new GetOpeFronteraByIdQuery(id);
        var opeFrontera = await _mediator.Send(query);

        if (opeFrontera == null)
            return NotFound();

        return Ok(opeFrontera);
    }

    [HttpPut(Name = "UpdateOpeFrontera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeFronteraCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeFrontera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeFronteraCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
