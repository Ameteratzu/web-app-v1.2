using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetTiposImpactosList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Route("api/v1")]
public class ImpactoClasificadoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImpactoClasificadoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("tipos-impactos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene los tipos de impactos clasificados")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTiposImpactos()
    {
        var query = new GetTiposImpactosListQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
