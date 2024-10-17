using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendioDetalles;
public class GetIncendioDetallesListQuery: IRequest<IReadOnlyList<IncendioDetalleVm>>
{
    public int IdIncendio { get; set; }

    public GetIncendioDetallesListQuery(int idIncendio)
    {
        IdIncendio = idIncendio;
    }
}
