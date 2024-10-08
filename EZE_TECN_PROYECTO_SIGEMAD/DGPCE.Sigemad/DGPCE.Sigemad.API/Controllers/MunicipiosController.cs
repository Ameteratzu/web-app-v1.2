using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipioByIdProvincia;
using DGPCE.Sigemad.Domain.Modelos;
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
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de los municipios para una determinada provincia")]
        public async Task<ActionResult<IReadOnlyList<Municipio>>> GetMunicipiosByIdProvincia(int idProvincia)
        {
            var query = new GetMunicipioByIdProvinciaQuery(idProvincia);
            var listado = await _mediator.Send(query);

            if (listado.Count == 0)
                return NotFound();

            return Ok(listado);
        }
    }
}
