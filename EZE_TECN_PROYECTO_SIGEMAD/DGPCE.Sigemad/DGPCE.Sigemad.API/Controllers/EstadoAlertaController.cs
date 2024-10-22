using DGPCE.Sigemad.Application.Features.Alertas.Queries.GetEstadosAlertasList;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.DeleteAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EstadoAlertaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EstadoAlertaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost(Name = "CreateEstadoAlerta")]
        //[Authorize(Roles = "Administrator")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateEstadoAlerta([FromBody] CreateEstadoAlertaCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(Name = "UpdateEstadoAlerta")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateEstadoAlerta([FromBody] UpdateEstadoAlertaCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteEstadoAlerta")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteEstadoAlerta(Guid id)
        {
            var command = new DeleteEstadoAlertaCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination", Name = "ListadoEstadosAlertas")]
        [ProducesResponseType(typeof(PaginationVm<EstadosAlertasVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<EstadosAlertasVm>>> GetListadoEstadosAlertas(
        [FromQuery] GetEstadosAlertasListQuery paginationEstadosAlertasParams
    )
        {
            var paginationEstadosAlertas = await _mediator.Send(paginationEstadosAlertasParams);
            return Ok(paginationEstadosAlertas);
        }

    }
}
