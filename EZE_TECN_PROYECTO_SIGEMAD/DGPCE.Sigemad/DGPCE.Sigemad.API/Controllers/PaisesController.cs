﻿using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.Paises.Queries.GetPaisesList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
    [Route("api/v1/paises")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaisesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene el listado de paises")]
        public async Task<ActionResult<IReadOnlyList<Pais>>> GetAll()
        {
            var query = new GetPaisesListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }
    }
}
