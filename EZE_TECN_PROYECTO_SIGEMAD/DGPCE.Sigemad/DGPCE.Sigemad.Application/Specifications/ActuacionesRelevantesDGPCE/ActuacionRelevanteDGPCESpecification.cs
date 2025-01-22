using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCESpecification : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCESpecification(int id)
      : base(d => d.Id == id && d.Borrado == false)
    {
        AddInclude(d => d.EmergenciaNacional);
        AddInclude(d => d.DeclaracionesZAGEP);
        AddInclude(d => d.ConvocatoriaCECOD);
        AddInclude(d => d.ActivacionPlanEmergencias);
        AddInclude(d => d.ActivacionSistemas);
        AddInclude("ActivacionPlanEmergencias.TipoPlan");
        AddInclude("ActivacionPlanEmergencias.PlanEmergencia");
        AddInclude("ActivacionPlanEmergencias.Archivo");
    }
}
