using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.DeleteOpeLineasMaritimas;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Queries.GetOpeLineaMaritimaById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.CreateOpeLineasMaritimas;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.UpdateOpeLineasMaritimas;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Queries.GetOpeLineasMaritimasList;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-lineas-maritimas")]
[ApiController]
public class OpeLineasMaritimasController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeLineasMaritimasController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpeLineaMaritima")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeLineaMaritimaResponse>> Create([FromBody] CreateOpeLineaMaritimaCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeLineaMaritima>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeLineaMaritimaVm>>> GetOpeLineasMaritimas(
        [FromQuery] GetOpeLineasMaritimasListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de línea marítima de OPE por id")]
    public async Task<ActionResult<OpeLineaMaritima>> GetById(int id)
    {
        var query = new GetOpeLineaMaritimaByIdQuery(id);
        var opeLineaMaritima = await _mediator.Send(query);

        if (opeLineaMaritima == null)
            return NotFound();

        return Ok(opeLineaMaritima);
    }

    [HttpPut(Name = "UpdateOpeLineaMaritima")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeLineaMaritimaCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeLineaMaritima")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeLineaMaritimaCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
