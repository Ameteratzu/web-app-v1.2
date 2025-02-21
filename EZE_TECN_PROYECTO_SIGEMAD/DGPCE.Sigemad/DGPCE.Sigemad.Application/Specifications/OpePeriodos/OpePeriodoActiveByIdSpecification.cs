using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.OpePeriodos;
public class OpePeriodoActiveByIdSpecification : BaseSpecification<OpePeriodo>
{
    public OpePeriodoActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
