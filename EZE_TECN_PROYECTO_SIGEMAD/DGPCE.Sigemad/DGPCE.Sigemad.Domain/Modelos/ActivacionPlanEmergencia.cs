using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class ActivacionPlanEmergencia : BaseEntity
{
    public int IdDireccionCoordinacionEmergencia { get; set; }
    public int IdTipoPlan { get; set; }
    public string NombrePlan { get; set; } = null!;
    public string AutoridadQueLoActiva { get; set; } = null!;
    public string? RutaDocumentoActivacion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Observaciones { get; set; }

    public virtual DireccionCoordinacionEmergencia DireccionCoordinacionEmergencia { get; set; } = null!;
    public virtual TipoPlan TipoPlan { get; set; } = null!;
}
