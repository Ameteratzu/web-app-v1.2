using DGPCE.Sigemad.Application.Features.ClasesSucesos.Quereis.GetClaseSucesosList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
        [Route("api/v1/clase-suceso")]
        [ApiController]
        public class ClaseSucesosController : ControllerBase
        {
            private readonly IMediator _mediator;

            public ClaseSucesosController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpGet]
            [ProducesResponseType((int)HttpStatusCode.OK)]
            [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
            [SwaggerOperation(Summary = "Obtiene todos los tipos de sucesos")]
            public async Task<IActionResult> GetAll()
            {
                var query = new GetClaseSucesosListQuery();
                var listado = await _mediator.Send(query);
                return Ok(listado);
            }
        }
    }

