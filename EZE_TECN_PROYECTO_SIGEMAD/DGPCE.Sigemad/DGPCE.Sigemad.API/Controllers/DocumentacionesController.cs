
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace DGPCE.Sigemad.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class DocumentacionesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentacionesController(IMediator mediator)
        {
            _mediator = mediator;

        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Crea la documentacion asociada a un incendio")]
        public async Task<ActionResult<ManageImpactoResponse>> CreateImpactos([FromBody] ManageDocumentacionesCommandList command)
        {
            var response = await _mediator.Send(command);
            return NoContent();
        }

    }
}