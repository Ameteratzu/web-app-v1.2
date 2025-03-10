namespace DGPCE.Sigemad.Application.Features.Parametros.Commands;
public class CreateParametroCommand: IEquatable<CreateParametroCommand>
{
    //public int? Id { get; set; }
    public int IdEstadoIncendio { get; set; }
    public DateTime? FechaFinal { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public int? IdPlanEmergencia { get; set; }

    public int? IdFaseEmergencia { get; set; }

    public int? IdPlanSituacion { get; set; }
    public int? IdSituacionEquivalente { get; set; }

    public bool Equals(CreateParametroCommand? other)
    {
        if (other is null)
        {
            return false;
        }

        return IdEstadoIncendio == other.IdEstadoIncendio &&
            FechaFinal == other.FechaFinal &&
            SuperficieAfectadaHectarea == other.SuperficieAfectadaHectarea &&
            IdPlanEmergencia == other.IdPlanEmergencia &&
            IdFaseEmergencia == other.IdFaseEmergencia &&
            IdPlanSituacion == other.IdPlanSituacion &&
            IdSituacionEquivalente == other.IdSituacionEquivalente;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CreateParametroCommand other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            IdEstadoIncendio,
            FechaFinal ?? default,
            SuperficieAfectadaHectarea ?? default,
            IdPlanEmergencia ?? default,
            IdFaseEmergencia ?? default,
            IdPlanSituacion ?? default,
            IdSituacionEquivalente ?? default);
    }

}
