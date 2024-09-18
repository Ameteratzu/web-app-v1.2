using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<int>> Create([FromBody] CreateIncendioCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
