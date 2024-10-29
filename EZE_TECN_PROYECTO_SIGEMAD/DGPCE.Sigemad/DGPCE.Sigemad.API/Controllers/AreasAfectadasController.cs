
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaById;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
 
    [Route("api/v1/areas-afectadas")]
    [ApiController]
    public class AreasAfectadasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AreasAfectadasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateAreaAfectada")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CreateAreaAfectadaResponse>> Create([FromBody] CreateAreaAfectadaCommand command)
        {
            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }


        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Busqueda de area afectada por id")]
        public async Task<ActionResult<Incendio>> GetById(int id)
        {
            var query = new GetAreaAfectadaByIdQuery(id);
            var areaAfectada = await _mediator.Send(query);

            if (areaAfectada == null)
                return NotFound();

            return Ok(areaAfectada);
        }

        [HttpGet]
        [Route("evolucion/{idEvolucion}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Obtiene la lista de área afectada por idEvolucion")]
        public async Task<ActionResult<IReadOnlyList<CaracterMedio>>> GetAreasAfectadasPorIdEvolucion(int idEvolucion)
        {
            var query = new GetAreasAfectadasByIdEvolucionQuery(idEvolucion);
            var listado = await _mediator.Send(query);
            if (listado.Count == 0)
                return NotFound();

            return Ok(listado);
        }
    }


}
