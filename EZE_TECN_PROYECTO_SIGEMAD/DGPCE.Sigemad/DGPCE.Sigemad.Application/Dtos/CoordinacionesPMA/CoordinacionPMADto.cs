using DGPCE.Sigemad.Application.Dtos.Municipios;
using DGPCE.Sigemad.Application.Dtos.Provincias;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
public class CoordinacionPMADto
{
    public int Id { get; set; }

    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public ProvinciaDto Provincia { get; set; }
    public MunicipioDto Municipio { get; set; }
    public string Lugar { get; set; }
    public string? Observaciones { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public bool EsEliminable { get; set; }
}
