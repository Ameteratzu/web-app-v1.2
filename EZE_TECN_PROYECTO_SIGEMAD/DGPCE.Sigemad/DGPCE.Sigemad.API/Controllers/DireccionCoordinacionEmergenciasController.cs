using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasByIdIncendioList;
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
        [SwaggerOperation(Summary = "Obtiene todas la lista general de DireccionCoordinacionEmergencia")]
        public async Task<ActionResult<IReadOnlyList<DireccionCoordinacionEmergenciaVm>>> GetAll()
        {
            var query = new GetDireccionCoordinacionEmergenciasListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }


        [HttpGet]
        [Route("incendio/{idIncendio}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene el listado de las DireccionCoordinacionEmergencia para un determinado incendio")]
        public async Task<ActionResult<IReadOnlyList<DireccionCoordinacionEmergenciaVm>>> GetDireccionCoordinacionEmergenciaByIdIncendio(int idIncendio)
        {
            var query = new GetDCEByIdIncendioListQuery(idIncendio);
            var listado = await _mediator.Send(query);

            if (listado.Count == 0)
                return NotFound();

            return Ok(listado);
        }
    }
}
