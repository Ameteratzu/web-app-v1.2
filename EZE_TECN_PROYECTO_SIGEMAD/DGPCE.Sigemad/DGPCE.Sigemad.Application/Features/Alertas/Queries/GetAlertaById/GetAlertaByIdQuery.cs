using DGPCE.Sigemad.Application.Features.Alertas.Queries.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Alertas.Queries.GetAlertaById
{
    public class GetAlertaByIdQuery : IRequest<AlertaVm>
    {
        public Guid? Id { get; set; }

        public GetAlertaByIdQuery(string id)
        {
            Id = new Guid(id);
        }


    }
}
