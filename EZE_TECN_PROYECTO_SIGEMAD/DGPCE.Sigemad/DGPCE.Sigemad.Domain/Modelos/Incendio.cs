using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Incendio : BaseDomainModel<int>
{
    public Incendio() { }

    public int IdSuceso { get; set; }
    public int IdTerritorio { get; set; }
    public int IdPais { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public int IdEstadoSuceso { get; set; }

    public string Denominacion { get; set; } = null!;

    public decimal? UtmX { get; set; }

    public decimal? UtmY { get; set; }

    public int? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }
    public string? Contenido { get; set; }

    public string? Comentarios { get; set; }

    public int IdClaseSuceso { get; set; }

    public bool CoordenadasReales { get; set; }
    public DateTime FechaInicio { get; set; }
    public string? RutaMapaRiesgo { get; set; }


    public virtual ClaseSuceso ClaseSuceso { get; set; } = null!;

    public virtual Municipio Municipio { get; set; } = null!;

    public virtual Provincia Provincia { get; set; } = null!;

    public virtual Suceso Suceso { get; set; } = null!;
    public virtual Territorio Territorio { get; set; } = null!;

    public virtual EstadoSuceso EstadoSuceso { get; set; } = null!;
    public virtual Pais Pais { get; set; } = null!;

}
