using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
public class CreateOrUpdateCoordinacionPmaDto
{
    public int? Id { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Lugar { get; set; } = string.Empty;
    public Geometry? GeoPosicion { get; set; }
    public string? Observaciones { get; set; }
}
