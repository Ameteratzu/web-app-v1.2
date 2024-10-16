using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateImpactoEvolucionCommand command)
    {
        var response = await _mediator.Send(command);
        //return CreatedAtAction(nameof(Get), new {id = response.Id}, response);
        return Ok(response);
    }

    

}
