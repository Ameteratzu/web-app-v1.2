using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class Parametro : BaseDomainModel<int>
{
    public int IdEstadoIncendio { get; set; }

    public DateTime? FechaFinal { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public int? IdPlanEmergencia { get; set; }
    public virtual PlanEmergencia PlanEmergencia { get; set; } = null!;

    public int? IdFaseEmergencia { get; set; }
    public virtual FaseEmergencia FaseEmergencia { get; set; } = null!;

    public int? IdPlanSituacion { get; set; }
    public virtual PlanSituacion PlanSituacion { get; set; } = null!;


    public virtual Evolucion Evolucion { get; set; } = null!;

    public virtual EstadoIncendio EstadoIncendio { get; set; } = null!;
}
