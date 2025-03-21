using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.DeleteOpeDatosFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatoFronteraById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.UpdateOpeDatosFronteras;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatosFronterasList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-datos-fronteras")]
[ApiController]
public class OpeDatosFronterasController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeDatosFronterasController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpeDatoFrontera")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeDatoFronteraResponse>> Create([FromBody] CreateOpeDatoFronteraCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeDatoFrontera>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeDatoFronteraVm>>> GetOpeDatosFronteras(
        [FromQuery] GetOpeDatosFronterasListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de dato de frontera de OPE por id")]
    public async Task<ActionResult<OpeDatoFrontera>> GetById(int id)
    {
        var query = new GetOpeDatoFronteraByIdQuery(id);
        var opeDatoFrontera = await _mediator.Send(query);

        if (opeDatoFrontera == null)
            return NotFound();

        return Ok(opeDatoFrontera);
    }

    [HttpPut(Name = "UpdateOpeDatoFrontera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeDatoFronteraCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeDatoFrontera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeDatoFronteraCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
