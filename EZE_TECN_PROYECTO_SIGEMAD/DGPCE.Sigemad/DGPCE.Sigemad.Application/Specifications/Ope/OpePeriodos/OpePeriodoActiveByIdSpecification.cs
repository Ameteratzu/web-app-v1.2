using DGPCE.Sigemad.Domain.Modelos.Ope;

namespace DGPCE.Sigemad.Application.Specifications.Ope.OpePeriodos;
public class OpePeriodoActiveByIdSpecification : BaseSpecification<OpePeriodo>
{
    public OpePeriodoActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
