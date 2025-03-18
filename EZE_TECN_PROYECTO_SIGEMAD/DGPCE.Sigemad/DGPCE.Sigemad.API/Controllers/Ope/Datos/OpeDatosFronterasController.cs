using DGPCE.Sigemad.Application.Dtos.Ope.Datos;
using DGPCE.Sigemad.Application.Dtos.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.ManageOtraInformaciones;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/ope-datos-fronteras")]
[ApiController]
public class OpeDatosFronterasController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeDatosFronterasController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("lista")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear lista de datos de una frontera de OPE")]
    public async Task<ActionResult<ManageOpeDatoFronteraResponse>> CreateLista([FromBody] ManageOpeDatoFronteraCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
