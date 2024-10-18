using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetTiposImpactosList;
public class GetTiposImpactosListQuery : IRequest<IReadOnlyList<string>>
{
}
