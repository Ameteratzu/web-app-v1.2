using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.CreateOtrasInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtrasInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.UpdateOtrasInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;
[Route("api/v1/otras-informaciones")]
[ApiController]
public class OtraInformacionController : ControllerBase
{
    private readonly IMediator _mediator;

    public OtraInformacionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CrearOtraInformacion")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear otra información")]
    public async Task<ActionResult<CreateOtraInformacionResponse>> Create([FromBody] CreateOtraInformacionCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOtraInformacionById), new { id = response.Id }, response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtener otra información mediante Id")]
    public async Task<ActionResult> GetOtraInformacionById(int id)
    {
        var query = new GetOtraInformacionByIdQuery(id);
        var otraInformacion = await _mediator.Send(query);

        if (otraInformacion == null)
        {
            return NotFound();
        }
        return Ok(otraInformacion);
    }

    [HttpDelete("detalles/{idDetalleOtraInformacion}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Eliminar un detalle de otra información mediante Id")]
    public async Task<ActionResult> Delete(int idDetalleOtraInformacion)
    {
        var command = new DeleteDetalleOtraInformacionCommand { IdDetalleOtraInformacion = idDetalleOtraInformacion };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("detalles", Name = "UpdateOtraInformacion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Actualizar detalle otra información")]
    public async Task<ActionResult> Update([FromBody] UpdateDetalleOtraInformacionCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
