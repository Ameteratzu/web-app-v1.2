using DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList;
using DGPCE.Sigemad.Application.Features.Provincias.Quereis.GetProvinciasByIdCCAAList;
using DGPCE.Sigemad.Application.Features.Provincias.Quereis.GetProvinciasList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProvinciasController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProvinciasController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene el listado de las provincias")]
        public async Task<IActionResult> GetProvincias()
        {
            var query = new GetProvinciasListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }


        [HttpGet]
        [Route("{idCcaa}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene el listado de las provincias para una determinada comunidad autonoma")]
        public async Task<IActionResult> GetProvinciasByIdCcaa(int idCcaa)
        {
            var query = new GetProvinciasByIdCCAAListQuery(idCcaa);
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }

    }
}
