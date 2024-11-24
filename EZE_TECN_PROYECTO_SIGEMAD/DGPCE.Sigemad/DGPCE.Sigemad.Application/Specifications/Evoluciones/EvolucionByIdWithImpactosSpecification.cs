using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionByIdWithImpactosSpecification : BaseSpecification<Evolucion>
{
    public EvolucionByIdWithImpactosSpecification(int id)
     : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(e => e.Impactos);
    }

}
