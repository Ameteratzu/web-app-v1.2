using DGPCE.Sigemad.Application.Features.EstadosAlertas.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Alertas.Queries.GetEstadosAlertasList
{
    public class GetEstadosAlertasListQuery : PaginationBaseQuery, IRequest<PaginationVm<EstadosAlertasVm>>
    {
    }
}
