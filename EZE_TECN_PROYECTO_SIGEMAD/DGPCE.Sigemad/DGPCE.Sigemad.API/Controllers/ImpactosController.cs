using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateListaImpactos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ImpactosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImpactosController(IMediator mediator)
    {
        _mediator = mediator;
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

    
}
