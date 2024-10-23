using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoPlanes.Quereis.GetTipoPlanesList;
public class GetTipoPlanesListQuery : IRequest<IReadOnlyList<TipoPlan>>
{
}
