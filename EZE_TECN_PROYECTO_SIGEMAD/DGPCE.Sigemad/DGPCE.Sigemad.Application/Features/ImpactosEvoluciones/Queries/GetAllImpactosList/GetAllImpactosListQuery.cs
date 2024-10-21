using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Queries.GetImpactosEvolucionesList;
public class GetAllImpactosListQuery: IRequest<IReadOnlyList<ImpactoEvolucion>>
{
}
