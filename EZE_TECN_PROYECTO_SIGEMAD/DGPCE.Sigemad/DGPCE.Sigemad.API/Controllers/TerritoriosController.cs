using DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TerritoriosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TerritoriosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene todos los tipos de territorios")]
        public async Task<IActionResult> GetTerritorio()
        {
            var query = new GetTerritoriosListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }

    }
}