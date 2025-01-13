using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace DGPCE.Sigemad.API.Controllers
{
    [Authorize]
    [Route("api/v1/actuaciones-relevantes")]
    [ApiController]
    public class ActuacionesRelevantesDGPCEController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ActuacionesRelevantesDGPCEController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpPost("emergencia-nacional")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ManageEmergenciaNacionalResponse>> Create([FromBody] ManageEmergenciasNacionalesCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
