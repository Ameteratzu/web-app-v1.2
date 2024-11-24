using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionCoordinacionEmergenciaWithCoordinacionPma : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaWithCoordinacionPma(int id)
        : base(d => d.Id == id && d.Borrado == false)
    {
        AddInclude(d => d.CoordinacionesPMA);
    }
}
