using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactoEvolucionById;
public class GetImpactoByIdQuery: IRequest<ImpactoEvolucion>
{
    public int Id { get; set; }

    public GetImpactoByIdQuery(int id)
    {
        Id = id;
    }
}
