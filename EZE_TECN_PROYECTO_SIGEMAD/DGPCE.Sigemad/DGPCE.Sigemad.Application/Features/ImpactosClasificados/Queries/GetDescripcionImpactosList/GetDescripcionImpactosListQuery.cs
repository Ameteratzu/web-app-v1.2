using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
public class GetDescripcionImpactosListQuery : IRequest<IReadOnlyList<ImpactoClasificadoDescripcionVm>>
{
}

