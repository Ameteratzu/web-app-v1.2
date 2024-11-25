using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class UpdateEvolucionWithImpactosSpecification : BaseSpecification<Evolucion>
{
    public UpdateEvolucionWithImpactosSpecification(int id)
        : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(e => e.Impactos);
    }
}
