using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;

public class OpeDatosFronterasForCountingSpecification : BaseSpecification<OpeDatoFrontera>
{
    public OpeDatosFronterasForCountingSpecification(OpeDatosFronterasSpecificationParams request)
        : base(opeDatoFrontera =>
        (!request.Id.HasValue || opeDatoFrontera.Id == request.Id) &&
        opeDatoFrontera.Borrado != true
        )
    {

    }
}
