using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Queries.GetIntervencionesByEvolucionIdList;
public class GetIntervencionMediosByEvolucionIdListQuery : IRequest<IReadOnlyList<IntervencionMedio>>
{
    public int IdEvolucion { get; set; }

    public GetIntervencionMediosByEvolucionIdListQuery(int idEvolucion)
    {
        IdEvolucion = idEvolucion;
    }
}
