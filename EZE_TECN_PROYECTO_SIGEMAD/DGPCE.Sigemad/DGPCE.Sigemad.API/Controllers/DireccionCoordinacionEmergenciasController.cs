using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasList;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{

    [Route("api/v1/direcciones-coordinaciones-emergencias")]
    [ApiController]
    public class DireccionCoordinacionEmergenciasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DireccionCoordinacionEmergenciasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene todas la lista general de DireccionCoordinacionEmergencia")]
        public async Task<ActionResult<IReadOnlyList<DireccionCoordinacionEmergenciaVm>>> GetAll()
        {
            var query = new GetDireccionCoordinacionEmergenciasListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }
    }
}
