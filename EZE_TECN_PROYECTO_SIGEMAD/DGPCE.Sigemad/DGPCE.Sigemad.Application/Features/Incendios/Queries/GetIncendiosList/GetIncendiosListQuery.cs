﻿using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList
{
    public class GetIncendiosListQuery : PaginationBaseQuery, IRequest<PaginationVm<Incendio>>
    {
    }
}
