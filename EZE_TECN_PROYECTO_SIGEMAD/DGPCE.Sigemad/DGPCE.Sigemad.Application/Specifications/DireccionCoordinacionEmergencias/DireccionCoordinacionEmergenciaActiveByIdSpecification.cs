using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;



public class DireccionCoordinacionEmergenciaActiveByIdSpecification : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaActiveByIdSpecification(int id)
        : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(e => e.ActivacionPlanEmergencia);
    }
}
