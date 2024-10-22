using DGPCE.Sigemad.Application.Features.Alertas.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Alertas.Queries.GetAlertasListByEstado
{
    public class GetAlertasListQuery : PaginationBaseQuery, IRequest<PaginationVm<AlertaVm>>
    {
        public int? idEstado { get; set; }
        public DateTime? fechaAlerta { get; set; }
    }
}
