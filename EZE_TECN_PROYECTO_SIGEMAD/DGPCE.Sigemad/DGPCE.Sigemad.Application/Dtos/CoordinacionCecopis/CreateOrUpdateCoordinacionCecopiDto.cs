using DGPCE.Sigemad.Application.Helpers;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
public class CreateOrUpdateCoordinacionCecopiDto : IEquatable<CreateOrUpdateCoordinacionCecopiDto>
{
    public int? Id { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Lugar { get; set; }
    public string? Observaciones { get; set; }
    public Geometry? GeoPosicion { get; set; }


    public bool Equals(CreateOrUpdateCoordinacionCecopiDto? other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id &&
            FechaInicio == other.FechaInicio &&
            FechaFin == other.FechaFin &&
            IdProvincia == other.IdProvincia &&
            IdMunicipio == other.IdMunicipio &&
            string.Equals(Lugar, other.Lugar) &&
            string.Equals(Observaciones, other.Observaciones) &&
            GeoJsonValidatorUtil.AreGeometriesEqual(GeoPosicion, other.GeoPosicion);
    }

    public override bool Equals(object? obj)
    {
        if (obj is CreateOrUpdateCoordinacionCecopiDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            FechaInicio,
            FechaFin ?? default,
            IdProvincia,
            IdMunicipio,
            Lugar ?? string.Empty,
            Observaciones ?? string.Empty,
            GeoPosicion ?? default);
    }
}
