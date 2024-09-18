using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Incendio: BaseDomainModel
{
    public Incendio() { }


    public int IdSuceso { get; set; }
    public int IdTerritorio { get; set; }
    public int IdProvincia { get; set; }

    public int IdMunicipio { get; set; }

    public string Denominacion { get; set; } = null!;

    public double? UtmX { get; set; }

    public double? UtmY { get; set; }

    public int? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }

    public string? Comentarios { get; set; }

    public int IdClaseSuceso { get; set; }

    public bool CoordenadasReales { get; set; }

    public int IdPrevisionPeligroGravedad { get; set; }
    public DateTime FechaInicio { get; set; }
    public bool? Borrado { get; set; }

    public virtual ClaseSuceso IdClaseSucesoNavigation { get; set; } = null!;

    public virtual Municipio IdMunicipioNavigation { get; set; } = null!;

    public virtual NivelGravedad IdPrevisionPeligroGravedadNavigation { get; set; } = null!;

    public virtual Provincia IdProvinciaNavigation { get; set; } = null!;

    public virtual Suceso IdSucesoNavigation { get; set; } = null!;
    public virtual Territorio IdTerritorioNavigation { get; set; } = null!;
}
