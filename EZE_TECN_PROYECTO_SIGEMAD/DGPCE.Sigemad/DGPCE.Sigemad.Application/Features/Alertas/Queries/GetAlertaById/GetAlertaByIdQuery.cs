using DGPCE.Sigemad.Application.Features.Alertas.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Alertas.Queries.GetAlertaById
{
    public class GetAlertaByIdQuery : IRequest<AlertaVm>
    {
        public int? Id { get; set; }

        public GetAlertaByIdQuery(int id)
        {
            Id = id;
        }


    }
}
