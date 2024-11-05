using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;



public class DireccionCoordinacionEmergenciaByIdSpecification : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaByIdSpecification(int id)
        : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(e => e.ActivacionPlanEmergencia);
    }
}
