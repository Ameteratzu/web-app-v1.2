﻿using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;

public class GetTerritoriosListQuery: IRequest<IReadOnlyList<Territorio>>
{
}
