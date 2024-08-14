using DGPCE.Sigemad.Application.Features.Alertas.Queries.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Alertas.Queries.GetAlertasListByEstado
{
    public class GetAlertasListQuery : PaginationBaseQuery, IRequest<PaginationVm<AlertasVm>>
    {
        public Guid? idEstado { get; set; }
        public DateTime? fechaAlerta { get; set; }
    }
}
