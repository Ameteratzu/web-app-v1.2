using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Provincia
{
    public Provincia() { }
    public int Id { get; set; }

    public int IdCcaa { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? UtmX { get; set; }

    public int? UtmY { get; set; }

    public string? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }

    public virtual Ccaa IdCcaaNavigation { get; set; } = null!;

    public virtual ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
}
