using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactosByEvolucionIdList;
public class GetImpactosByEvolucionIdListQuery : IRequest<IReadOnlyList<ImpactoEvolucion>>
{
    public int IdEvolucion { get; set; }

    public GetImpactosByEvolucionIdListQuery(int idEvolucion)
    {
        IdEvolucion = idEvolucion;
    }
}
