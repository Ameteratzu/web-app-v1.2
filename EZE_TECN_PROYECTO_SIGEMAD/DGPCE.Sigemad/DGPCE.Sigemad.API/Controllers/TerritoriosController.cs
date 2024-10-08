using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;
using DGPCE.Sigemad.Domain.Modelos;
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
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de territorios")]
        public async Task<ActionResult<IReadOnlyList<Territorio>>> GetTerritorio()
        {
            var query = new GetTerritoriosListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }

    }
}