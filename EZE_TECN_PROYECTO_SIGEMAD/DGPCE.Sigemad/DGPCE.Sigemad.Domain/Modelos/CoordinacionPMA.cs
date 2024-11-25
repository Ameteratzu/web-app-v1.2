using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;
public class CoordinacionPMA : BaseDomainModel<int>
{
    public int IdDireccionCoordinacionEmergencia { get; set; }
    public virtual DireccionCoordinacionEmergencia DireccionCoordinacionEmergencia { get; set; }

    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }

    public int IdProvincia { get; set; }
    public Provincia Provincia { get; set; }

    public int IdMunicipio { get; set; }
    public Municipio Municipio { get; set; }

    public string Lugar { get; set; }
    public string? Observaciones { get; set; }
    public Geometry? GeoPosicion { get; set; }
}
