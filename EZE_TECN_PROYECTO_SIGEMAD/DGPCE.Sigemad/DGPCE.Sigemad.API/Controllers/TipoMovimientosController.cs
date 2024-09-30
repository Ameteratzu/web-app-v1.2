﻿using DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadosIncendioList;
using DGPCE.Sigemad.Application.Features.TipoMovimientos.Quereis.GetTipoMovimientosList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

    namespace DGPCE.Sigemad.API.Controllers
    {
        [ApiController]
        [Route("api/v1/[controller]")]
        public class TipoMovimientosController : ControllerBase
        {
            private readonly IMediator _mediator;

            public TipoMovimientosController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpGet]
            [ProducesResponseType((int)HttpStatusCode.OK)]
            [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
            [SwaggerOperation(Summary = "Obtiene todos los tipos de movimientos")]
            public async Task<IActionResult> GetAll()
            {
                var query = new GetTipoMovimientosLisQuery();
                var listado = await _mediator.Send(query);
                return Ok(listado);
            }

        }
    }


