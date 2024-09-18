using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using DGPCE.Sigemad.Application.Features.Incendios.Queries;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    }
}
