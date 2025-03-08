﻿using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.DireccionCoordinaciones;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Features.CoordinacionCecopis.Commands.CreateCoordinacionCecopi;
using DGPCE.Sigemad.Application.Features.CoordinacionesPma.Commands.CreateOrUpdateCoordinacionPma;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.DeleteByRegistroActualizacion;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionEmergencia;
using DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/direcciones-coordinaciones-emergencias")]
[ApiController]
public class DireccionCoordinacionEmergenciasController : ControllerBase
{
    private readonly IMediator _mediator;

    public DireccionCoordinacionEmergenciasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{idRegistroActualizacion:int}", Name = "DeleteDireccionCoordinacionEmergencia")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int idRegistroActualizacion)
    {
        //var command = new DeleteDireccionCoordinacionEmergenciaCommand { Id = id };
        var command = new DeleteDireccionByIdRegistroActualizacionCommand { IdRegistroActualizacion = idRegistroActualizacion };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<DireccionCoordinacionEmergenciaDto>> GetDireccionCoordinacionEmergencia(
    [FromQuery] int? idRegistroActualizacion,
    [FromQuery] int idSuceso)
    {
        var query = new GetDireccionCoordinacionEmergenciaQuery
        {
            IdRegistroActualizacion = idRegistroActualizacion,
            IdSuceso = idSuceso
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpPost("direcciones")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateDireccionResponse>> CreateDirecciones([FromBody] CreateOrUpdateDireccionCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("coordinaciones-cecopi")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateCoordinacionCecopiResponse>> CreateCoordinacionesCecopi([FromBody] CreateOrUpdateCoordinacionCecopiCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("coordinaciones-pma")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOrUpdateCoordinacionPmaResponse>> CreateCoordinacionesPma([FromBody] CreateOrUpdateCoordinacionPmaCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
