using DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadosIncendioList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EstadoIncendioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EstadoIncendioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene todos los estados de incendio")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetEstadosIncendioListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }

    }
}
