using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/registros")]
[ApiController]
public class RegistrosController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegistrosController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
