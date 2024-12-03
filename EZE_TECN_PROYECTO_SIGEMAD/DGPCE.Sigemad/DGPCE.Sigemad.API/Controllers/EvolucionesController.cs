using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.ManageEvoluciones;
using DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucionById;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class EvolucionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EvolucionesController(IMediator mediator)
    {
        _mediator = mediator;

    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Obtener de evolución por id")]
    public async Task<ActionResult<EvolucionDto>> GetById(int id)
    {
        var query = new GetEvolucionByIdQuery(id);
        var EvolucionVm = await _mediator.Send(query);

        return Ok(EvolucionVm);
    }

    [HttpPost("areas-afectadas")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateAreaAfectadaResponse>> Create([FromBody] CreateOrUpdateAreaAfectadaCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }


    [HttpPost(("Datos"), Name = "CreateEvolucion")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageEvolucionResponse>> Create([FromBody] ManageEvolucionCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPost("impactos")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear lista de impactos de una evolucion (Consecuencia/Actuacion)")]
    public async Task<ActionResult<ManageImpactoResponse>> CreateImpactos([FromBody] ManageImpactosCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("{id:int}", Name = "DeleteEvolucion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteEvolucionesCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    //[HttpPut(Name = "UpdateEvolucion")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<ActionResult> Update([FromBody] UpdateEvolucionCommand command)
    //{
    //    await _mediator.Send(command);
    //    return NoContent();
    //}



    //[HttpGet]
    //[Route("{idIncendio}")]
    //[ProducesResponseType((int)HttpStatusCode.OK)]
    //[ProducesResponseType((int)HttpStatusCode.NotFound)]
    //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    //[SwaggerOperation(Summary = "Obtiene el listado de las evoluciones para un determinado incendio")]
    //public async Task<ActionResult<IReadOnlyList<EvolucionVm>>> GetEvolucionesByIdIncendio(int idIncendio)
    //{
    //    var query = new GetEvolucionesByIdIncendioListQuery(idIncendio);
    //    var listado = await _mediator.Send(query);
    //    return Ok(listado);
    //}


    //[HttpGet]
    //[Route("{idEvolucion}/areas-afectadas")]
    //[ProducesResponseType((int)HttpStatusCode.OK)]
    //[ProducesResponseType((int)HttpStatusCode.NotFound)]
    //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    //[SwaggerOperation(Summary = "Obtiene la lista de área afectada por idEvolucion")]
    //public async Task<ActionResult<IReadOnlyList<AreaAfectadaDto>>> GetAreasAfectadasPorIdEvolucion(int idEvolucion)
    //{
    //    var query = new GetAreasAfectadasByIdEvolucionQuery(idEvolucion);
    //    var listado = await _mediator.Send(query);
    //    return Ok(listado);
    //}

    //[HttpGet("{idEvolucion}/impactos")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[SwaggerOperation(Summary = "Listar todos los impactos por Id de Evolucion (Consecuencia/Actuacion)")]
    //public async Task<IActionResult> GetImpactosByIdEvolucion(int idEvolucion)
    //{
    //    var query = new GetImpactosByEvolucionIdListQuery(idEvolucion);
    //    var listado = await _mediator.Send(query);
    //    return Ok(listado);
    //}

}
