using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;

public class Evolucion : BaseDomainModel<int>
{
    public int IdIncendio { get; set; }
    public DateTime? FechaHoraEvolucion { get; set; }
    public int? IdEntradaSalida { get; set; }
    public int? IdMedio { get; set; }
    public int? IdTipoRegistro { get; set; }
    public string? Observaciones { get; set; }
    public string? Prevision { get; set; }
    public int? IdEstadoIncendio { get; set; }
    public string? PlanEmergenciaActivado { get; set; }
    public int? IdSituacionOperativa { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }
    public DateTime? FechaFinal { get; set; }

    public virtual Medio Medio { get; set; } = null!;
    public virtual EntradaSalida EntradaSalida { get; set; } = null!;
    public virtual Incendio Incendio { get; set; } = null!;
    public virtual EstadoIncendio EstadoIncendio { get; set; } = null!;
    public virtual TipoRegistro TipoRegistro { get; set; } = null!;
    public virtual SituacionOperativa SituacionOperativa { get; set; } = null!;


    public ICollection<EvolucionProcedenciaDestino>? EvolucionProcedenciaDestinos { get; set; } = null;

}
