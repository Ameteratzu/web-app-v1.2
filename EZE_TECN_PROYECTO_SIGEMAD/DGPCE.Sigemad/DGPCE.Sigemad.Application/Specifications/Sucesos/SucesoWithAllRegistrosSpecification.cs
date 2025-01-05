using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Sucesos;
public class SucesoWithAllRegistrosSpecification : BaseSpecification<Suceso>
{
    public SucesoWithAllRegistrosSpecification(int idSuceso)
        : base(s => s.Id == idSuceso && s.Borrado == false)
    {
        AddInclude(i => i.Evoluciones.Where(dir => !dir.Borrado));
        AddInclude(i => i.Documentaciones.Where(dir => !dir.Borrado));
        AddInclude(i => i.OtraInformaciones.Where(dir => !dir.Borrado));
        AddInclude(i => i.DireccionCoordinacionEmergencias.Where(dir => !dir.Borrado));
        AddInclude(i => i.SucesoRelacionados.Where(dir => !dir.Borrado));
    }
}
