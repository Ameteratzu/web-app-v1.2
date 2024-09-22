using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.DeleteIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosNacionalesById;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IncendiosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IncendiosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> Create([FromBody] CreateIncendioCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationVm<Incendio>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<Incendio>>> GetIncendios(
            [FromQuery] GetIncendiosListQuery query)
        {
            var pagination = await _mediator.Send(query);
            return Ok(pagination);
        }

        [HttpGet]
        [Route("busquedaIncendioNacional/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Busqueda de incendios en territorio nacional")]
        public async Task<ActionResult<Incendio>> GetIncendioNacional(int id)
        {
            var query = new GetIncendiosNacionalesByIdQuery(id);
            var incendio = await _mediator.Send(query);

            if (incendio == null)
                return NotFound();

            return Ok(incendio);
        }

        [HttpPut(Name = "UpdateIncendio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromBody] UpdateIncendioCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteIncendio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteIncendioCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

    }
}
