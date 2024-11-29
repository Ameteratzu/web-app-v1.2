
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentaciones;
using DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;
using DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDetalleDocumentacionesById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;


namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
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
        public async Task<ActionResult<CreateOrUpdateDocumentacionResponse>> Create([FromBody] ManageDocumentacionesCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Obtener los detalles de la documentacion por id")]
    public async Task<ActionResult<DocumentacionDto>> GetById(int id)
    {
        var query = new GetDetalleDocumentacionesByIdQuery(id);
        var documentacionVm = await _mediator.Send(query);

        return Ok(documentacionVm);
    }

    [HttpDelete("{id:int}", Name = "DeleteDocumentacion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteDocumentacionesCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

}