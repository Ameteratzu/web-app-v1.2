using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionCoordinacionEmergenciaWithCoordinacionCecopis : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaWithCoordinacionCecopis(int id)
        : base(d => d.Id == id && d.Borrado == false)
    {
        AddInclude(d => d.CoordinacionesCecopi);
    }
}
