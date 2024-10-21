using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Quereis.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Quereis.GetTipoIntervencionMediosList;
public class GetTipoIntervencionMediosListQuery : IRequest<IReadOnlyList<TipoIntervencionMedioVm>>
{
}
