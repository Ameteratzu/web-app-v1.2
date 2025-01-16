using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetGruposImpactosList;
public class GetGruposImpactosListQuery : IRequest<IReadOnlyList<string>>
{
    public string? Tipo { get; set; }
}
