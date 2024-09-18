using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Municipio
{
    public Municipio() { }

    public int Id { get; set; }

    public int IdProvincia { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? UtmX { get; set; }

    public int? UtmY { get; set; }

    public string? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }

    public virtual Provincia IdProvinciaNavigation { get; set; } = null!;
    public virtual ICollection<Incendio> Incendios { get; set; } = new List<Incendio>();
}
