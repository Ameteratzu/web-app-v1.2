﻿using DGPCE.Sigemad.API.Constants;
using DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DGPCE.Sigemad.API.Controllers;

    [Route("api/v1/caracter-medios")]
    [ApiController]
    public class CaracterMediosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CaracterMediosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Tags = new[] { SwaggerTags.Maestros }, Summary = "Obtiene la lista general de caracter de medios")]
        public async Task<ActionResult<IReadOnlyList<CaracterMedio>>> GetAll()
        {
            var query = new GetCaracterMediosListQuery();
            var listado = await _mediator.Send(query);
            return Ok(listado);
        }
    }

