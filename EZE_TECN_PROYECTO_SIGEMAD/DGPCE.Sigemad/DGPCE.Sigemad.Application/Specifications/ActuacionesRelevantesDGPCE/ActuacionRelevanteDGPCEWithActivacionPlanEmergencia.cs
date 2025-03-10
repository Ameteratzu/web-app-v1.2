using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
internal class ActuacionRelevanteDGPCEWithActivacionPlanEmergencia : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCEWithActivacionPlanEmergencia(ActuacionRelevanteDGPCESpecificationParams @params)
        : base(d =>
        (!@params.Id.HasValue || d.Id == @params.Id) &&
        (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
        d.Borrado == false)
    {
        AddInclude(d => d.ActivacionPlanEmergencias); 
        AddInclude("ActivacionPlanEmergencias.TipoPlan");
        AddInclude("ActivacionPlanEmergencias.PlanEmergencia");
        AddInclude("ActivacionPlanEmergencias.Archivo");
    }
}
