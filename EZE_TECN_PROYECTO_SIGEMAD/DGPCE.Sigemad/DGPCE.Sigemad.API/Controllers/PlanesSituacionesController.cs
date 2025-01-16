﻿using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Queries.GetPlanesSituacionesListByIdPlanIdFase;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;
[Authorize]
[ApiController]
[Route("/api/v1/plan-situacion-emergencia")]
public class PlanesSituacionesController : Controller
{
    private readonly IMediator _mediator;

    public PlanesSituacionesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet()]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de las fases de emergencia completas o filtradas por el plan de emergencia")]
    public async Task<ActionResult<IReadOnlyList<PlanSituacionVm>>> GetPlanSituacionesByIdPlanEmergeniaAndIdFaseEmergencia([FromQuery] int? idPlanEmergencia, [FromQuery] int? idFaseEmergencia)
    {
        var query = new GetPlanesSituacionesByIdPlanIdFaseListQuery(idPlanEmergencia, idFaseEmergencia);
        var listado = await _mediator.Send(query);
        return Ok(listado);
    }
}