using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers
{
  
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
}
