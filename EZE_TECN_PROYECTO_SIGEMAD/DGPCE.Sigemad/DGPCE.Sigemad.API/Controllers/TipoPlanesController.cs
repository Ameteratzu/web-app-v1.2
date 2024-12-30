using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergenciasByIdTipoPlan;
using DGPCE.Sigemad.Application.Features.TipoPlanes.Quereis.GetTipoPlanesList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
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



    [HttpGet("{idTipoPlan}/planes-emergencias")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene la lista de planes de emergencias asociado al tipo de plan")]
    public async Task<ActionResult<IReadOnlyList<TipoPlan>>> GetPlanesEmergenciaByIdTipoPlan(int idTipoPlan)
    {
        var query = new GetPlanesEmergenciasListByIdTipoPlanQuery(idTipoPlan);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}