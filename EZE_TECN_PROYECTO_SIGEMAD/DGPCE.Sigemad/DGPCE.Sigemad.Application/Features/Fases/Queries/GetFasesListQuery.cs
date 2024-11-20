using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Fases.Queries;
public class GetFasesListQuery : IRequest<IReadOnlyList<Fase>>
{
}
