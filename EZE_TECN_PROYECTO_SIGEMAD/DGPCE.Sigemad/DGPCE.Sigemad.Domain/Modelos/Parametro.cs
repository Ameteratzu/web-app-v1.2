using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class Parametro : BaseDomainModel<int>
{
    public int IdEstadoIncendio { get; set; }

    public DateTime? FechaFinal { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public string? PlanEmergenciaActivado { get; set; }

    public int? IdFase { get; set; }

    public int? IdSituacionOperativa { get; set; }

    public int? IdSituacionEquivalente { get; set; }

    public virtual Evolucion Evolucion { get; set; } = null!;

    public virtual EstadoIncendio EstadoIncendio { get; set; } = null!;

    public virtual Fase Fase { get; set; } = null!;

    public virtual SituacionOperativa SituacionOperativa { get; set; } = null!;

    public virtual SituacionEquivalente SituacionEquivalente { get; set; } = null!;
}
