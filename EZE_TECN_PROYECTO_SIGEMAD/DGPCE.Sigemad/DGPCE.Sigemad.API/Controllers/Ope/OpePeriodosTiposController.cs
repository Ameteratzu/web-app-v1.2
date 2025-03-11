using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Ope.OpePeriodosTipos.Queries.GetOpePeriodosList;
using DGPCE.Sigemad.Domain.Modelos.Ope;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers.ope;

[Authorize]
[Route("api/v1/ope-periodos-tipos")]
[ApiController]
public class OpePeriodosTiposController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpePeriodosTiposController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de tipos de periodos para la OPE")]
    public async Task<ActionResult<IReadOnlyList<OpePeriodoTipo>>> GetAll()
    {
        var query = new GetOpePeriodosTiposListQuery { };
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }



}
