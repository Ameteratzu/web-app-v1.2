using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TipoPlanes.Quereis.GetTipoPlanesList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Route("api/v1/tipos-planes")]
[ApiController]
public class TipoPlanesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoPlanesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas la lista general de TipoPlan")]
    public async Task<ActionResult<IReadOnlyList<TipoPlan>>> GetAll()
    {
        var query = new GetTipoPlanesListQuery();
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}