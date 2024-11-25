using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Application.Features.Archivos.Commands.CreateFile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DGPCE.Sigemad.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ArchivosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArchivosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] ContextFile context)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("El archivo no ex válido");
        }

        var command = new CreateFileCommand
        {
            NombreOriginal = file.FileName,
            Tipo = file.ContentType,
            Extension = Path.GetExtension(file.FileName),
            PesoEnBytes = file.Length,
            Archivo = file.OpenReadStream(),
            Context = context
        };

        var respuesta = await _mediator.Send(command);

        return Ok(respuesta);
    }
}