using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionByIdWithAreaAfectadaSpecification : BaseSpecification<Evolucion>
{
    public EvolucionByIdWithAreaAfectadaSpecification(int id)
     : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(e => e.AreaAfectadas);
        AddInclude(e => e.Impactos);
    }

}
