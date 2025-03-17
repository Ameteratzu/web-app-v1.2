using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePeriodosTipos.Queries.GetOpePeriodosTiposList
{

    public class GetOpePeriodosTiposListQuery : IRequest<IReadOnlyList<OpePeriodoTipo>>
    {
    }
}
