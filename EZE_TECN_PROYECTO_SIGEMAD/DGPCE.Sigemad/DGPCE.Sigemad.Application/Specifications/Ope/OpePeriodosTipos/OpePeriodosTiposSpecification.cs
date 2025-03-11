using DGPCE.Sigemad.Domain.Modelos.Ope;

namespace DGPCE.Sigemad.Application.Specifications.Ope.OpePeriodosTipos;

public class OpePeriodosTiposSpecification : BaseSpecification<OpePeriodoTipo>
{
    public OpePeriodosTiposSpecification()
       : base(opePeriodoTipo => opePeriodoTipo.Borrado != true)
    {

    }
}
