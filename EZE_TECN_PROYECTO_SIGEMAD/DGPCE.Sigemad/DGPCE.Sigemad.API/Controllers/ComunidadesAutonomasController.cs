using DGPCE.Sigemad.Application.Features.CCAA.Queries.GetComunidadesAutonomasList;
using DGPCE.Sigemad.Application.Features.CCAA.Queries.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ComunidadesAutonomasController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ComunidadesAutonomasController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene el listado de las comunidades autonomas y sus provincias")]
        public async Task<IActionResult> GetComunidadesAutonomas()
        {
            var query = new GetComunidadesAutonomasListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }

    }
}
