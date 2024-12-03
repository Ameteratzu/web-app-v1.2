using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
public class CoordinacionPMADto
{
    public int Id { get; set; }

    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public Provincia Provincia { get; set; }
    public Municipio Municipio { get; set; }
    public string Lugar { get; set; }
    public string? Observaciones { get; set; }
    public Geometry? GeoPosicion { get; set; }
}
