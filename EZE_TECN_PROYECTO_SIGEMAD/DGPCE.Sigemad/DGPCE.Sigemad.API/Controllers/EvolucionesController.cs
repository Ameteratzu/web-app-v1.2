using DGPCE.Sigemad.Application.Features.Evoluciones.Quereis.GetEvolucionesByIdIncendioList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EvolucionesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EvolucionesController(IMediator mediator)
        {
            _mediator = mediator;

        }


        [HttpGet]
        [Route("{idIncendio}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene el listado de las evoluciones para un determinado incendio")]
        public async Task<IActionResult> GetEvolucionesByIdIncendio(int idIncendio)
        {
            var query = new GetEvolucionesByIdIncendioListQuery(idIncendio);
            var listado = await _mediator.Send(query);

            if (listado.Count == 0)
                return NotFound();

            return Ok(listado);
        }

    }
}
