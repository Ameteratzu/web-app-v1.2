using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateListaImpactos;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactosByEvolucionIdList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ImpactosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImpactosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateListaImpactoEvolucion")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear lista de impactos de una evolucion (Consecuencia/Actuacion)")]
    public async Task<ActionResult<CreateListaImpactosResponse>> Create([FromBody] CreateListaImpactosCommand command)
    {
        var response = await _mediator.Send(command);
        //return CreatedAtAction(nameof(GetImpactoById), new { id = response.Id }, response);
        return Ok(response);
    }

    [HttpPut(Name = "UpdateListaImpactoEvolucion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Actualizar impacto de una evolucion (Consecuencia/Actuacion)")]
    public async Task<ActionResult<UpdateListaImpactosResponse>> Update([FromBody] UpdateListaImpactosCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("/api/v1/evoluciones/{idEvolucion}/impactos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Listar todos los impactos por Id de Evolucion (Consecuencia/Actuacion)")]
    public async Task<IActionResult> GetImpactosByIdEvolucion(int idEvolucion)
    {
        var query = new GetImpactosByEvolucionIdListQuery(idEvolucion);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

}
