using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SituacionesEquivalentes.Queries;
public class GetSituacionesEquivalentesListQuery : IRequest<IReadOnlyList<SituacionEquivalente>>
{
}
