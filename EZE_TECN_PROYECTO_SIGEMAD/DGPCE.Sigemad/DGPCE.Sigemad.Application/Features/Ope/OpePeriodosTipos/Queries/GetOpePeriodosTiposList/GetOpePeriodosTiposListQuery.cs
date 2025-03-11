using DGPCE.Sigemad.Domain.Modelos.Ope;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.OpePeriodosTipos.Queries.GetOpePeriodosList
{

    public class GetOpePeriodosTiposListQuery : IRequest<IReadOnlyList<OpePeriodoTipo>>
    {
    }
}
