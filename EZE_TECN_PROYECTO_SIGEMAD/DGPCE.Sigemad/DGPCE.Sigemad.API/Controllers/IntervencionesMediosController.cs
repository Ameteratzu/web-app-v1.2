using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.CreateIntervencionMedios;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.DeleteIntervencionMedios;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.UpdateIntervencionMedios;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Queries.GetIntervencionById;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Queries.GetIntervencionesByEvolucionIdList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;
[Route("api/v1/intervenciones-medios")]
[ApiController]
public class IntervencionMediosController : ControllerBase
{
    private readonly IMediator _mediator;

    public IntervencionMediosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateIntervencionMedio")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear una intervencion de medio")]
    public async Task<ActionResult<CreateIntervencionMedioResponse>> Create([FromBody] CreateIntervencionMedioCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetIntervencionById), new { id = response.Id }, response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtener intervencion mediante id")]
    public async Task<ActionResult<IntervencionMedio>> GetIntervencionById(int id)
    {
        var query = new GetIntervencionByIdQuery(id);
        var impacto = await _mediator.Send(query);
        return Ok(impacto);
    }

    [HttpPut(Name = "UpdateIntervencionMedio")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Actualizar intervencion de medio de una evolucion")]
    public async Task<ActionResult> Update([FromBody] UpdateIntervencionMedioCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("evolucion/{idEvolucion}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Listar las intervenciones de medios por Id de Evolucion")]
    public async Task<ActionResult<IReadOnlyList<IntervencionMedio>>> GetIntervencionesByIdEvolucion(int idEvolucion)
    {
        var query = new GetIntervencionMediosByEvolucionIdListQuery(idEvolucion);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }

    [HttpDelete("{id}", Name = "DeleteIntervencionMedio")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteIntervencionMedioCommand
        {
            Id = id
        };

        await _mediator.Send(command);

        return NoContent();
    }
}
