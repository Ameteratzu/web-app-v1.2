
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.DeleteAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaById;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;


[Route("api/v1/areas-afectadas")]
[ApiController]
public class AreasAfectadasController : ControllerBase
{
    private readonly IMediator _mediator;

    public AreasAfectadasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateAreaAfectada")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateAreaAfectadaResponse>> Create([FromBody] CreateAreaAfectadaCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut(Name = "UpdateAreaAfectada")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateAreaAfectadaCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteAreaAfectada")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteAreaAfectadaCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }


    [HttpGet("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de area afectada por id")]
    public async Task<ActionResult<Incendio>> GetById(int id)
    {
        var query = new GetAreaAfectadaByIdQuery(id);
        var areaAfectada = await _mediator.Send(query);

        return Ok(areaAfectada);
    }

    [HttpGet]
    [Route("evolucion/{idEvolucion}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Obtiene la lista de área afectada por idEvolucion")]
    public async Task<ActionResult<IReadOnlyList<CaracterMedio>>> GetAreasAfectadasPorIdEvolucion(int idEvolucion)
    {
        var query = new GetAreasAfectadasByIdEvolucionQuery(idEvolucion);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}
