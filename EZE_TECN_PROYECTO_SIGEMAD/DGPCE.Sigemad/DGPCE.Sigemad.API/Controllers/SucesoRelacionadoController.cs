using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.CreateSucesosRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesosRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.UpdateSucesosRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesosRelacionadosByIdSucesoPrincipal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Route("api/v1/sucesos")]
[ApiController]
public class SucesoRelacionadoController : ControllerBase
{
    private readonly IMediator _mediator;

    public SucesoRelacionadoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{idSucesoPrincipal}/relacionados", Name = "CrearSucesoRelacionado")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crea un suceso relacionado a un suceso principal")]
    public async Task<ActionResult<CreateSucesoRelacionadoResponse>> Create(
        [FromRoute] int idSucesoPrincipal,
        [FromBody] CreateSucesoRelacionadoCommand command)
    {
        command.IdSucesoPrincipal = idSucesoPrincipal;
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSucesoRelacionadoById), new { id = response.Id }, response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtiene un suceso relacionado por su id")]
    public async Task<ActionResult> GetSucesoRelacionadoById(int id)
    {
        var query = new GetSucesoRelacionadoByIdQuery(id);
        var sucesoRelacionado = await _mediator.Send(query);
        if (sucesoRelacionado == null)
        {
            return NotFound();
        }
        return Ok(sucesoRelacionado);
    }

    [HttpGet("{idSucesoPrincipal}/relacionados")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtiene los sucesos relacionados por el idSucesoPrincipal")]
    public async Task<ActionResult> GetSucesosRelacionadosByIdSucesoPrincipal(int idSucesoPrincipal)
    {
        var query = new GetSucesosRelacionadosByIdSucesoPrincipalQuery(idSucesoPrincipal);
        var sucesoRelacionado = await _mediator.Send(query);
        if (sucesoRelacionado == null)
        {
            return NotFound();
        }
        return Ok(sucesoRelacionado);
    }

    [HttpPut("{idSucesoPrincipal}/relacionados/{idSucesoAsociado}", Name = "UpdateSucesoRelacionado")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Actualiza un suceso relacionado")]
    public async Task<ActionResult> Update(
        [FromRoute] int idSucesoPrincipal,
        [FromRoute] int idSucesoAsociado,
        [FromBody] UpdateSucesoRelacionadoCommand command)
    {
        command.IdSucesoAsociado = idSucesoAsociado;
        command.IdSucesoPrincipal = idSucesoPrincipal;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{idSucesoPrincipal}/relacionados/{idSucesoAsociado}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Elimina un suceso relacionado")]
    public async Task<ActionResult> Delete(
        [FromRoute] int idSucesoPrincipal,
        [FromRoute] int idSucesoAsociado)
    {
        var command = new DeleteSucesoRelacionadoCommand { IdSucesoPrincipal = idSucesoPrincipal, IdSucesoAsociado = idSucesoAsociado };
        await _mediator.Send(command);
        return NoContent();
    }


}
