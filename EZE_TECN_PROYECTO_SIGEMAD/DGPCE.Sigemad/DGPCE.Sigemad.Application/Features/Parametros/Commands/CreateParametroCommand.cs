namespace DGPCE.Sigemad.Application.Features.Parametros.Commands;
public class CreateParametroCommand
{
    public int IdEstadoIncendio { get; set; }
    public DateTime? FechaFinal { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public int? IdPlanEmergencia { get; set; }

    public int? IdFaseEmergencia { get; set; }

    public int? IdPlanSituacion { get; set; }
    public int? IdSituacionEquivalente { get; set; }
}
