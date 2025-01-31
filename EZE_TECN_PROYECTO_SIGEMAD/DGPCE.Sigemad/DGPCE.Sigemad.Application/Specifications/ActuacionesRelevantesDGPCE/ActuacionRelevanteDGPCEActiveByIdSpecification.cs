using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCEActiveByIdSpecification : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCEActiveByIdSpecification(ActuacionRelevanteDGPCESpecificationParams request)
    : base(ActuacionRelevanteDGPCyE =>
      (!request.Id.HasValue || ActuacionRelevanteDGPCyE.Id == request.Id) &&
     (!request.IdSuceso.HasValue || ActuacionRelevanteDGPCyE.IdSuceso == request.IdSuceso) &&
     (ActuacionRelevanteDGPCyE.Borrado == false))
    {
        if (request.Id.HasValue)
        {
            AddInclude(d => d.EmergenciaNacional);

            AddInclude(d => d.ActivacionPlanEmergencias.Where(dir => !dir.Borrado));
            AddInclude("ActivacionPlanEmergencias.TipoPlan");
            AddInclude("ActivacionPlanEmergencias.PlanEmergencia");
            AddInclude("ActivacionPlanEmergencias.Archivo");

            AddInclude(d => d.DeclaracionesZAGEP.Where(dir => !dir.Borrado));

            AddInclude(d => d.ActivacionSistemas.Where(dir => !dir.Borrado));

            AddInclude(d => d.ConvocatoriasCECOD.Where(dir => !dir.Borrado));

            AddInclude(d => d.NotificacionesEmergencias.Where(dir => !dir.Borrado));
            AddInclude(d => d.MovilizacionMedios.Where(dir => !dir.Borrado));


            AddInclude("MovilizacionMedios.Pasos.PasoMovilizacion");
            AddInclude("MovilizacionMedios.Pasos.SolicitudMedio");
            AddInclude("MovilizacionMedios.Pasos.SolicitudMedio.ProcedenciaMedio");
            AddInclude("MovilizacionMedios.Pasos.SolicitudMedio.Archivo");

            AddInclude("MovilizacionMedios.Pasos.TramitacionMedio");
            AddInclude("MovilizacionMedios.Pasos.TramitacionMedio.DestinoMedio");

            AddInclude("MovilizacionMedios.Pasos.CancelacionMedio");
            AddInclude("MovilizacionMedios.Pasos.OfrecimientoMedio");

            AddInclude("MovilizacionMedios.Pasos.AportacionMedio");
            AddInclude("MovilizacionMedios.Pasos.AportacionMedio.Capacidad");
            AddInclude("MovilizacionMedios.Pasos.AportacionMedio.TipoAdministracion");

            AddInclude("MovilizacionMedios.Pasos.DespliegueMedio");
            AddInclude("MovilizacionMedios.Pasos.DespliegueMedio.Capacidad");

            AddInclude("MovilizacionMedios.Pasos.FinIntervencionMedio");
            AddInclude("MovilizacionMedios.Pasos.FinIntervencionMedio.Capacidad");

            AddInclude("MovilizacionMedios.Pasos.LlegadaBaseMedio");
            AddInclude("MovilizacionMedios.Pasos.LlegadaBaseMedio.Capacidad");
        }
    }

}
