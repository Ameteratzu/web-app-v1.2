using DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetComunidadesAutonomas()
        {
            var query = new GetComunidadesAutonomasListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }

    }
}
