
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
 
    [Route("api/v1/areas-afectadas")]
    [ApiController]
    public class AreasAfectadasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AreasAfectadasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("evolucion/{idEvolucion}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene la lista de área afectada por idEvolucion")]
        public async Task<ActionResult<IReadOnlyList<CaracterMedio>>> GetAreasAfectadasPorIdEvolucion(int idEvolucion)
        {
            var query = new GetAreaAfectadaListQuery(idEvolucion);
            var listado = await _mediator.Send(query);
            if (listado.Count == 0)
                return NotFound();

            return Ok(listado);
        }
    }


}
