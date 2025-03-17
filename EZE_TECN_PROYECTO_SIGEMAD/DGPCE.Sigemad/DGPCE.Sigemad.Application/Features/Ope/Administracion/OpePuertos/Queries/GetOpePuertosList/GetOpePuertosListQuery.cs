using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Queries.GetOpePuertosList
{

    public class GetOpePuertosListQuery : OpePuertosSpecificationParams, IRequest<PaginationVm<OpePuertoVm>>
    {
    }
}
