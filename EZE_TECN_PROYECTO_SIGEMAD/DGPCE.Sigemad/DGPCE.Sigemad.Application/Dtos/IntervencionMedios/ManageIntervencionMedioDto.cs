using DGPCE.Sigemad.Application.Helpers;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
public class ManageIntervencionMedioDto : IEquatable<ManageIntervencionMedioDto>
{
    public int? Id { get; set; }
    public int IdCaracterMedio { get; set; }
    public string? Descripcion { get; set; }
    public string? MedioNoCatalogado { get; set; }
    public int NumeroCapacidades { get; set; }
    public int IdTitularidadMedio { get; set; }
    public string? Titular { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public string? Observaciones { get; set; }
    public List<ManageDetalleIntervencionMedioDto> DetalleIntervencionMedios { get; set; } = new();


    public bool Equals(ManageIntervencionMedioDto? other)
    {
        if (other is null) return false;

        return Id == other.Id &&
               IdCaracterMedio == other.IdCaracterMedio &&
               string.Equals(Descripcion, other.Descripcion) &&
               string.Equals(MedioNoCatalogado, other.MedioNoCatalogado) &&
               NumeroCapacidades == other.NumeroCapacidades &&
               IdTitularidadMedio == other.IdTitularidadMedio &&
               string.Equals(Titular, other.Titular) &&
               FechaHoraInicio == other.FechaHoraInicio &&
               FechaHoraFin == other.FechaHoraFin &&
               IdProvincia == other.IdProvincia &&
               IdMunicipio == other.IdMunicipio &&
               GeoJsonValidatorUtil.Equals(GeoPosicion, other.GeoPosicion) &&
               string.Equals(Observaciones, other.Observaciones) &&
               DetalleIntervencionMedios.SequenceEqual(other.DetalleIntervencionMedios);
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageIntervencionMedioDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = HashCode.Combine(
        Id,
        IdCaracterMedio,
        Descripcion ?? string.Empty,
        MedioNoCatalogado ?? string.Empty,
        NumeroCapacidades,
        IdTitularidadMedio,
        Titular ?? string.Empty,
        FechaHoraInicio
    );

        hash = HashCode.Combine(
            hash,
            FechaHoraFin ?? default,
            IdProvincia,
            IdMunicipio,
            GeoPosicion?.GetHashCode() ?? new Point(0, 0).GetHashCode(),
            Observaciones ?? string.Empty,
            DetalleIntervencionMedios.Count
        );

        return hash;
    }
}
