using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class ActuacionRelevanteDGPCE : BaseDomainModel<int>
{
    public int IdSuceso { get; set; }
    public Suceso Suceso { get; set; } = null!;
    public virtual EmergenciaNacional? EmergenciaNacional { get; set; }
    public virtual List<ActivacionPlanEmergencia> ActivacionPlanEmergencias { get; set; } = new();
    public virtual List<DeclaracionZAGEP>? DeclaracionesZAGEP { get; set; } = null;

    public virtual List<ActivacionSistema>? ActivacionSistemas { get; set; } = null;

    public virtual List<ConvocatoriaCECOD>? ConvocatoriaCECOD { get; set; } = null;
    public virtual List<MovilizacionMedio> MovilizacionMedios { get; set; } = new();

}
