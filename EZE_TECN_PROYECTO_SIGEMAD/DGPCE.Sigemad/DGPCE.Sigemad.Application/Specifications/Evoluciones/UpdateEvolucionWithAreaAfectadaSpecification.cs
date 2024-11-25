using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class UpdateEvolucionWithAreaAfectadaSpecification : BaseSpecification<Evolucion>
{
    public UpdateEvolucionWithAreaAfectadaSpecification(int id)
        : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(e => e.AreaAfectadas);
    }
}
