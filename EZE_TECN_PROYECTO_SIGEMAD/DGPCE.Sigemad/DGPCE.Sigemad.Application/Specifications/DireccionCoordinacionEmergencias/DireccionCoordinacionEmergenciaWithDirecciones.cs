using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionCoordinacionEmergenciaWithDirecciones : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaWithDirecciones(int id)
        : base(d => d.Id == id && d.Borrado == false)
    {
        AddInclude(d => d.Direcciones);
    }
}
