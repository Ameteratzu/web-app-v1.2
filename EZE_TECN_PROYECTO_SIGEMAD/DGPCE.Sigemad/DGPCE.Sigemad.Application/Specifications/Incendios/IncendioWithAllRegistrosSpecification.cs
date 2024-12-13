using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Incendios;
public class IncendioWithAllRegistrosSpecification : BaseSpecification<Incendio>
{
    public IncendioWithAllRegistrosSpecification(int idIncendio)
        : base(i => i.Id == idIncendio && i.Borrado == false)
    {

        AddInclude(i => i.Evoluciones.Where(dir => !dir.Borrado));
        AddInclude(i => i.Documentaciones.Where(dir => !dir.Borrado));
        AddInclude(i => i.OtraInformaciones.Where(dir => !dir.Borrado));
        AddInclude(i => i.DireccionCoordinacionEmergencias.Where(dir => !dir.Borrado));
    }
}

