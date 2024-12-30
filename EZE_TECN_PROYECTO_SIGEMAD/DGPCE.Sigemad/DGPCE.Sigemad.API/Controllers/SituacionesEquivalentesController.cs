using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.SituacionesEquivalentes.Queries;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/situaciones-equivalentes")]
public class SituacionesEquivalentesController : Controller
{
    private readonly IMediator _mediator;

    public SituacionesEquivalentesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado completo de situaciones equivalentes")]
    public async Task<ActionResult<IReadOnlyList<FaseEmergencia>>> GetAll()
    {
        var query = new GetSituacionesEquivalentesListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}