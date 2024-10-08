using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosEvolucion.Queries.GetEstadosEvolucionList
{
    public class GetEstadosEvolucionListQuery : IRequest<IReadOnlyList<EstadoEvolucion>>
    {
    }
}
