using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Create;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Delete;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Update;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.DireccionCoordinacionEmergenciasById;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasByIdIncendioList;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasList;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
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


        [HttpPost(Name = "CreateDireccionCoordinacionEmergencia")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CreateDireccionCoordinacionEmergenciasCommand>> Create([FromBody] CreateDireccionCoordinacionEmergenciasCommand command)
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDireccionCoordinacionEmergencianById), new { id = response.Id }, response);
        }

        [HttpDelete("{id:int}", Name = "DeleteDireccionCoordinacionEmergencia")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteDireccionCoordinacionEmergenciaCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut(Name = "UpdateDireccionCoordinacionEmergencia")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromBody] UpdateDireccionCoordinacionEmergenciaCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obtener DireccionCoordinacionEmergencia mediante id")]
        public async Task<ActionResult<DireccionCoordinacionEmergencia>> GetDireccionCoordinacionEmergencianById(int id)
        {
            var query = new GetDireccionCoordinacionEmergenciasById(id);
            var impacto = await _mediator.Send(query);
            return Ok(impacto);
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
