using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [Route("api/v1/entidad-menor")]
    [ApiController]
    public class EntidadesMenoresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EntidadesMenoresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas la lista general de tipos de entidad menor")]
        public async Task<ActionResult<IReadOnlyList<ClaseSuceso>>> GetAll()
        {
            var query = new GetEntidadMenorListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }
    }
}
