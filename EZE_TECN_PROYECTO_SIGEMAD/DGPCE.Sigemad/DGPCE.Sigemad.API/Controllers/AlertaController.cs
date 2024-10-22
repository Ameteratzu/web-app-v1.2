using DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.Alertas.Queries.GetAlertaById;
using DGPCE.Sigemad.Application.Features.Alertas.Queries.GetAlertasListByEstado;
using DGPCE.Sigemad.Application.Features.Alertas.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.DeleteAlertas;
using DGPCE.Sigemad.Application.Features.Shared;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AlertaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlertaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("pagination", Name = "ListadoAlertas")]
        [ProducesResponseType(typeof(PaginationVm<AlertaVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<AlertaVm>>> GetListadoAlertas(
                [FromQuery] GetAlertasListQuery paginationAlertasParams
            )
        {
            var paginationAlertas = await _mediator.Send(paginationAlertasParams);
            return Ok(paginationAlertas);
        }

        [HttpGet("ObtenerAlerta", Name = "ObtenerAlerta")]
        [ProducesResponseType(typeof(AlertaVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AlertaVm>> GetAlertaById(int id ) 
        {
            var query = new GetAlertaByIdQuery(id);
            var alerta = await _mediator.Send(query);
            return Ok(alerta);
        }

        [HttpPost(Name = "CreateAlerta")]
        //[Authorize(Roles = "Administrator")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateAlerta([FromBody] CreateAlertaCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(Name = "UpdateAlerta")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateAlerta([FromBody] UpdateAlertaCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteAlerta")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteAlerta(Guid id)
        {
            var command = new DeleteEstadoAlertaCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

    }
}
