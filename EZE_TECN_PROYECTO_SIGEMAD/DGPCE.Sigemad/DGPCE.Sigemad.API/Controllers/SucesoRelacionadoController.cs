using DGPCE.Sigemad.Application.Dtos.SucesoRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesosRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/sucesos-relacionados")]
[ApiController]
public class SucesoRelacionadoController : ControllerBase
{
    private readonly IMediator _mediator;

    public SucesoRelacionadoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtiene un suceso relacionado por su id")]
    public async Task<ActionResult<SucesoRelacionadoVm>> GetSucesoRelacionadoById(int id)
    {
        var query = new GetSucesoRelacionadoByIdQuery(id);
        var sucesoRelacionado = await _mediator.Send(query);
        return Ok(sucesoRelacionado);
    }

    [HttpPost("lista")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear lista de sucesos relacionados")]
    public async Task<ActionResult<ManageSucesoRelacionadoResponse>> CreateLista([FromBody] ManageSucesoRelacionadosCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Eliminar un suceso relacionado por su id")]
    public async Task<ActionResult> DeleteSucesoRelacionado(int id)
    {
        var command = new DeleteSucesosRelacionadosCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

}
