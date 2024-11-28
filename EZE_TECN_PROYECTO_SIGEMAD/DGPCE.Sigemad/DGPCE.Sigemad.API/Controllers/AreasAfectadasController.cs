
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.DeleteAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaById;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/areas-afectadas")]
[ApiController]
public class AreasAfectadasController : ControllerBase
{
    private readonly IMediator _mediator;

    public AreasAfectadasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut(Name = "UpdateAreaAfectada")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update(int idEvolucion, [FromBody] UpdateAreaAfectadaCommand command)
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
    public async Task<ActionResult<AreaAfectadaDto>> GetById(int id)
    {
        var query = new GetAreaAfectadaByIdQuery(id);
        var areaAfectada = await _mediator.Send(query);

        return Ok(areaAfectada);
    }
}
