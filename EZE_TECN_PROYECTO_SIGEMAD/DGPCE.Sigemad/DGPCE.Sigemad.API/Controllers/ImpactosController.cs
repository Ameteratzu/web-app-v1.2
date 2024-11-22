using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
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

}
