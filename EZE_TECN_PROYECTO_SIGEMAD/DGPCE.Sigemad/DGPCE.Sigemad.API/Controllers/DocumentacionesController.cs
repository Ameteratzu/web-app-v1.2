
using MediatR;
using Microsoft.AspNetCore.Mvc;


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


     

    }
}