using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.TipoNotificaciones.Queries.GetTipoNotificacionesList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{

    [Route("api/v1/tipo-notificacion")]
    [ApiController]
    public class TipoNotificacionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TipoNotificacionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todos los tipos de notificacion")]
        public async Task<ActionResult<IReadOnlyList<TipoNotificacion>>> GetAll()
        {
            var query = new GetTipoNotificacionesListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }
    }
}