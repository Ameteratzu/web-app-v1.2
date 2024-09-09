using DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ComunidadAutonomaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ComunidadAutonomaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        
        [HttpGet("pagination", Name = "ListadoComunidadesAutonomas")]
        [ProducesResponseType(typeof(PaginationVm<ComunidadesAutonomasVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<ComunidadesAutonomasVm>>> GetListadoEstadosAlertas(
        [FromQuery] GetComunidadesAutonomasListQuery paginationComunidadesAutonomasParams
    )
        {
            var paginationComunidadesAutonomas = await _mediator.Send(paginationComunidadesAutonomasParams);
            return Ok(paginationComunidadesAutonomas);
        }

    }
}
