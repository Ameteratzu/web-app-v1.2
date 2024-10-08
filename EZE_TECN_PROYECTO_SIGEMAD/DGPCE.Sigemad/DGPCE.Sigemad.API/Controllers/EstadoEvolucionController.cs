using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.EstadosEvolucion.Queries.GetEstadosEvolucionList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("/api/v1/estados-evolucion")]
    public class EstadoEvolucionController : Controller
    {
        private readonly IMediator _mediator;

        public EstadoEvolucionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los estados de evolución")]
        public async Task<ActionResult<IReadOnlyList<EstadoEvolucion>>> GetAll()
        {
            var query = new GetEstadosEvolucionListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }
    }
}
