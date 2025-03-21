using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuntosControlCarreteras.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuntosControlCarreteras;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuntosControlCarreteras.Queries.GetOpePuntosControlCarreterasList
{

    public class GetOpePuntosControlCarreterasListQuery : OpePuntosControlCarreterasSpecificationParams, IRequest<PaginationVm<OpePuntoControlCarreteraVm>>
    {
    }
}
