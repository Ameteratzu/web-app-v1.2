using DGPCE.Sigemad.Application.Features.Municipios.Quereis.GetMunicipioByIdProvincia;
using DGPCE.Sigemad.Application.Features.Provincias.Quereis.GetProvinciasList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MunicipiosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MunicipiosController(IMediator mediator)
        {
            _mediator = mediator;

        }


        [HttpGet]
        [Route("{idProvincia}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene el listado de los municipios para una determinada provincia")]
        public async Task<IActionResult> GetMunicipiosByIdProvincia(int idProvincia)
        {
            var query = new GetMunicipioByIdProvinciaQuery(idProvincia);
            var listado = await _mediator.Send(query);

            if (listado.Count == 0)
                return NotFound();

            return Ok(listado);
        }
    }
}
