using DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Provincias.Quereis.GetProvinciasList
{
    public class GetProvinciasListQuery : IRequest<IReadOnlyList<ProvinciaSinMunicipiosConIdComunidadVm>>
    {
    }
}
