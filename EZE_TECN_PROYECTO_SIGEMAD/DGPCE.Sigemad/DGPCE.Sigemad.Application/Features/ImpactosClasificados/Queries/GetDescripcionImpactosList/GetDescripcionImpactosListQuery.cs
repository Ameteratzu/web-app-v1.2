using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
public class GetDescripcionImpactosListQuery : IRequest<IReadOnlyList<TipoImpactoVm>>
{
}

