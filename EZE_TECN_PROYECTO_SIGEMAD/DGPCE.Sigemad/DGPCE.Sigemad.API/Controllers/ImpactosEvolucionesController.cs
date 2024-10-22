using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.DeleteImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactoEvolucionById;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactosByEvolucionIdList;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactosEvolucionesList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;
[Route("api/v1/impactos-evoluciones")]
[ApiController]
public class ImpactosEvolucionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImpactosEvolucionesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateImpactoEvolucion")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear impacto de una evolucion (Consecuencia/Actuacion)")]
    public async Task<ActionResult<CreateImpactoEvolucionResponse>> Create([FromBody] CreateImpactoEvolucionCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetImpactoById), new { id = response.Id }, response);
    }

    [HttpPut(Name = "UpdateImpactoEvolucion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Actualizar impacto de una evolucion (Consecuencia/Actuacion)")]
    public async Task<ActionResult> Update([FromBody] UpdateImpactoEvolucionCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Listar todos los impactos (Consecuencia/Actuacion)")]
    public async Task<ActionResult<IReadOnlyList<ImpactoEvolucion>>> GetAll()
    {
        var query = new GetAllImpactosListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtener impacto mediante id (Consecuencia/Actuacion)")]
    public async Task<IActionResult> GetImpactoById(int id)
    {
        var query = new GetImpactoByIdQuery(id);
        var impacto = await _mediator.Send(query);
        return Ok(impacto);
    }

    [HttpGet("evolucion/{idEvolucion}")]
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

    [HttpDelete("{id}", Name = "DeleteImpactoEvolucion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteImpactoEvolucionCommand
        {
            Id = id
        };

        await _mediator.Send(command);

        return NoContent();
    }

}
