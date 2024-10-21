using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ClasificacionMedios.Quereis;
public class GetClasificacionMediosListQuery : IRequest<IReadOnlyList<ClasificacionMedio>>
{
}
