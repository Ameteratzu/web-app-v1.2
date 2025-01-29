using DGPCE.Sigemad.API.Models.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.ActuacionesRelevantes;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
using DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands.ManageActivacionSistema;
using DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Commands.DeleteActuacionRelevante;
using DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Quereis.ActuacionesRelevantesById;
using DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
using DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
using DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;


namespace DGPCE.Sigemad.API.Controllers;


[Route("api/v1/actuaciones-relevantes")]
[ApiController]
public class ActuacionesRelevantesDGPCEController : ControllerBase
{
    private readonly IMediator _mediator;
    public ActuacionesRelevantesDGPCEController(IMediator mediator)
    {
        _mediator = mediator;

    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtener Actuacion Relevante mediante id")]
    public async Task<ActionResult<ActuacionRelevanteDGPCEDto>> GetActuacionRelevanteDGPCEById(int id)
    {
        var query = new GetActuacionRelevanteDGPCEById(id);
        var impacto = await _mediator.Send(query);
        return Ok(impacto);
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Eliminar actuaci�n relevante por id")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteActuacionRelevanteCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("emergencia-nacional")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageEmergenciaNacionalResponse>> Create([FromBody] ManageEmergenciasNacionalesCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

 

    [HttpPost("activaciones-planes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Crear listado de activaciones de planes en Actuacion Relevante")]
    public async Task<ActionResult<ManageActivacionPlanEmergenciaResponse>> CreateActivacionPlan([FromForm] ManageActivacionPlanRequest request)
    {
        // Mapear desde el modelo de API al command
        var command = new ManageActivacionPlanEmergenciaCommand
        {
            IdActuacionRelevante = request.IdActuacionRelevante,
            IdSuceso = request.IdSuceso
        };

        // Procesar cada detalle y su archivo
        foreach (var detalle in request.ActivacionPlanes)
        {
            var detalleDto = new ManageActivacionPlanEmergenciaDto
            {
                Id = detalle.Id,
                IdTipoPlan = detalle.IdTipoPlan,
                IdPlanEmergencia = detalle.IdPlanEmergencia,
                TipoPlanPersonalizado = detalle.TipoPlanPersonalizado,
                PlanEmergenciaPersonalizado = detalle.PlanEmergenciaPersonalizado,
                FechaInicio = detalle.FechaInicio,
                FechaFin = detalle.FechaFin,
                Autoridad = detalle.Autoridad,
                Observaciones = detalle.Observaciones
            };

            if (detalle.Archivo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await detalle.Archivo.CopyToAsync(memoryStream); // Copia el contenido al MemoryStream
                    detalleDto.Archivo = new FileDto
                    {
                        Extension = Path.GetExtension(detalle.Archivo.FileName),
                        Length = detalle.Archivo.Length,
                        FileName = detalle.Archivo.FileName,
                        ContentType = detalle.Archivo.ContentType,
                        Content = memoryStream.ToArray() // Convierte el contenido a un arreglo de bytes
                    };
                }
            }

            command.ActivacionesPlanes.Add(detalleDto);
        }

        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("declaraciones-zagep/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageDeclaracionZAGEPResponse>> CreateDeclaracionZAGEP([FromBody] ManageDeclaracionesZAGEPCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }


    [HttpPost("activaciones-sistemas/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageActivacionSistemaResponse>> Create([FromBody] ManageActivacionSistemaCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }


    [HttpPost("convocatoria-cecod/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageDeclaracionZAGEPResponse>> CreateConvocatoriaCECOD([FromBody] ManageConvocatoriaCECODCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("notificaciones/lista")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ManageNotificacionEmergenciaResponse>> CreateNotificacionesEmergencia([FromBody] ManageNotificacionEmergenciaCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

}
