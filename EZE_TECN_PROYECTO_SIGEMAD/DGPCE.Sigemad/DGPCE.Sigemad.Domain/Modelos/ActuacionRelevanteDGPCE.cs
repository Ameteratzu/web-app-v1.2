using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class ActuacionRelevanteDGPCE : BaseDomainModel<int>
{
    public int IdSuceso { get; set; }
    public Suceso Suceso { get; set; } = null!;
    public virtual EmergenciaNacional? EmergenciaNacional { get; set; }
    public virtual List<ActivacionPlanEmergencia> ActivacionPlanEmergencias { get; set; } = new();
    public virtual List<DeclaracionZAGEP> DeclaracionesZAGEP { get; set; } = new();
    public virtual List<ActivacionSistema> ActivacionSistemas { get; set; } = new();
    public virtual List<ConvocatoriaCECOD> ConvocatoriasCECOD { get; set; } = new();
    public virtual List<NotificacionEmergencia> NotificacionesEmergencias { get; set; } = new();
    public virtual List<MovilizacionMedio> MovilizacionMedios { get; set; } = new();

}
