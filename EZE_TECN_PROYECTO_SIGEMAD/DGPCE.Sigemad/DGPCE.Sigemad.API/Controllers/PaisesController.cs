using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.CCAA.Queries.GetCCAAByIdPaisList;
using DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Queries.GetMunicipiosExtranjerosByIdPais;
using DGPCE.Sigemad.Application.Features.Paises.Queries.GetPaisesList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [Route("api/v1/paises")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaisesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de paises")]
        public async Task<ActionResult<IReadOnlyList<Pais>>> GetAll([FromQuery] bool excluirNacional = false)
        {
            var query = new GetPaisesListQuery { ExcluirNacional = excluirNacional };
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }

        [HttpGet("{idPais}/comunidades")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de comunidades de un pais")]
        public async Task<ActionResult<IReadOnlyList<Pais>>> GetComunidades(int idPais)
        {
            var query = new GetCCAAByIdPaisListQuery(idPais);
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }

        [HttpGet("{idPais}/municipios")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de municipios de un pais")]
        public async Task<ActionResult<IReadOnlyList<Pais>>> GetMunicipios(int idPais)
        {
            var query = new GetMunicipiosExtranjerosByIdPaisQuery(idPais);
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }
    }
}
