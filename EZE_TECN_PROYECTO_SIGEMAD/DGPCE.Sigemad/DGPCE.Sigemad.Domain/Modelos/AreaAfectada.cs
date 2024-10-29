
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AreaAfectada
{
    public int Id { get; set; }
    public int IdEvolucion { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public int IdEntidadMenor { get; set; }
    public Geometry? GeoPosicion { get; set; }

    public virtual Municipio Municipio { get; set; } = null!;
    public virtual Evolucion Evolucion { get; set; } = null!;
    public virtual Provincia Provincia { get; set; } = null!;
    public virtual EntidadMenor EntidadMenor { get; set; } = null!;

}
