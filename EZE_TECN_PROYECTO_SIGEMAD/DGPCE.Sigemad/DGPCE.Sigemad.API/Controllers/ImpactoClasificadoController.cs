using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetGruposImpactosList;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetTiposImpactosList;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
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

    [HttpGet("grupos-impactos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene los grupos de impactos clasificados")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetGruposImpactos()
    {
        var query = new GetGruposImpactosListQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("impactos")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene lista general de los impactos clasificados")]
    public async Task<ActionResult<IReadOnlyList<ImpactoClasificadoDescripcionVm>>> GetImpactos()
    {
        var query = new GetDescripcionImpactosListQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}
