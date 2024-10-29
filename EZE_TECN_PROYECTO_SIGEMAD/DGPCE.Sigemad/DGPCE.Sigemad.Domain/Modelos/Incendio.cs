using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Incendio : BaseDomainModel<int>
{
    public Incendio() { }

    public int IdSuceso { get; set; }
    public int IdTerritorio { get; set; }
    public int IdClaseSuceso { get; set; }
    public int IdEstadoSuceso { get; set; }
    public DateTime FechaInicio { get; set; }
    public string Denominacion { get; set; } = null!;
    public string NotaGeneral { get; set; } = null!;
    public string? RutaMapaRiesgo { get; set; }
    public decimal? UtmX { get; set; }
    public decimal? UtmY { get; set; }
    public int? Huso { get; set; }
    public Geometry? GeoPosicion { get; set; }


    public virtual Suceso Suceso { get; set; } = null!;
    public virtual Territorio Territorio { get; set; } = null!;
    public virtual ClaseSuceso ClaseSuceso { get; set; } = null!;
    public virtual EstadoSuceso EstadoSuceso { get; set; } = null!;

    public virtual IncendioNacional IncendioNacional { get; set; } = null!;
    public virtual IncendioExtranjero IncendioExtranjero { get; set; } = null!;
}
