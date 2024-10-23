using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoDireccionEmergencias.Quereis;
public class GetTipoDireccionEmergenciasListQuery : IRequest<IReadOnlyList<TipoDireccionEmergencia>>
{
}
