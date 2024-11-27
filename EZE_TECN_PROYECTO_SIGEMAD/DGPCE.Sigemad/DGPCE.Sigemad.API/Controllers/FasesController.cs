using DGPCE.Sigemad.API.Constants;using DGPCE.Sigemad.Application.Features.Fases.Queries;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/fases")]
public class FasesController : Controller
{
    private readonly IMediator _mediator;

    public FasesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado completo de fases")]
    public async Task<ActionResult<IReadOnlyList<Fase>>> GetAll()
    {
        var query = new GetFasesListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}