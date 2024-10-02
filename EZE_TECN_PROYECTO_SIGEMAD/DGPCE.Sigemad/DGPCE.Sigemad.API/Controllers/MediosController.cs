﻿using DGPCE.Sigemad.Application.Features.Medios.Quereis.GetMediosList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers
{
        [ApiController]
        [Route("api/v1/medios")]
        public class MediosController : ControllerBase
        {
            private readonly IMediator _mediator;

            public MediosController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpGet]
            [ProducesResponseType((int)HttpStatusCode.OK)]
            [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
            [SwaggerOperation(Summary = "Obtiene el listado de medios completo")]
            public async Task<IActionResult> GetAll()
            {
                var query = new GetMediosListQuery();
                var listado = await _mediator.Send(query);
                return Ok(listado);
            }

        }
    }

