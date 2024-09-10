﻿using DGPCE.Sigemad.Application.Features.Menus.Queries.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Menus.Queries.GetMenusList
{
    public class GetMenusListQuery: IRequest<IReadOnlyList<MenuItemVm>>
    {
    }
}
