using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.Impactos;
public class ManageImpactoDto : IEquatable<ManageImpactoDto>
{
    public int? Id { get; set; }
    public int IdImpactoClasificado { get; set; }
    public bool? Nuclear { get; set; }
    public int? ValorAD { get; set; }
    public int? Numero { get; set; }
    public string? Observaciones { get; set; }
    public DateTime? Fecha { get; set; }
    public DateTime? FechaHora { get; set; }
    public DateTime? FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public char? AlteracionInterrupcion { get; set; }
    public string? Causa { get; set; }
    public int? NumeroGraves { get; set; }
    public int? IdTipoDanio { get; set; }
    public Geometry? ZonaPlanificacion { get; set; }
    public int? NumeroUsuarios { get; set; }
    public int? NumeroIntervinientes { get; set; }
    public int? NumeroServicios { get; set; }
    public int? NumeroLocalidades { get; set; }

    public bool Equals(ManageImpactoDto? other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id &&
            IdImpactoClasificado == other.IdImpactoClasificado &&
            Nuclear == other.Nuclear &&
            ValorAD == other.ValorAD &&
            Numero == other.Numero &&
            string.Equals(Observaciones, other.Observaciones) &&
            Fecha == other.Fecha &&
            FechaHora == other.FechaHora &&
            FechaHoraInicio == other.FechaHoraInicio &&
            FechaHoraFin == other.FechaHoraFin &&
            AlteracionInterrupcion == other.AlteracionInterrupcion &&
            string.Equals(Causa, other.Causa) &&
            NumeroGraves == other.NumeroGraves &&
            IdTipoDanio == other.IdTipoDanio &&
            ZonaPlanificacion == other.ZonaPlanificacion &&
            NumeroUsuarios == other.NumeroUsuarios &&
            NumeroIntervinientes == other.NumeroIntervinientes &&
            NumeroServicios == other.NumeroServicios &&
            NumeroLocalidades == other.NumeroLocalidades;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageImpactoDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = HashCode.Combine(
            Id,
            IdImpactoClasificado,
            Nuclear,
            ValorAD,
            Numero,
            Observaciones ?? string.Empty,
            Fecha ?? default);

        hash = HashCode.Combine(
            hash,
            FechaHora ?? default,
            FechaHoraInicio ?? default,
            FechaHoraFin ?? default,
            AlteracionInterrupcion ?? default,
            Causa ?? string.Empty,
            NumeroGraves,
            IdTipoDanio);

        hash = HashCode.Combine(
            hash,
            ZonaPlanificacion ?? default,
            NumeroUsuarios,
            NumeroIntervinientes,
            NumeroServicios,
            NumeroLocalidades);

        return hash;
    }
}
