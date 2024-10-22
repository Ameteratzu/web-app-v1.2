using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;
public class IntervencionMedio : BaseDomainModel<int>
{
    public IntervencionMedio()
    {
        
    }

    public int IdEvolucion { get; set; }
    public int IdTipoIntervencionMedio { get; set; }
    public int IdCaracterMedio { get; set; }
    public int IdClasificacionMedio { get; set; }
    public int IdTitularidadMedio { get; set; }
    public int IdMunicipio { get; set; }
    public int Cantidad { get; set; }
    public string Unidad { get; set; }
    public string Titular { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public string? Observaciones { get; set; }

    public virtual Evolucion Evolucion { get; set; }
    public virtual TipoIntervencionMedio TipoIntervencionMedio { get; set; }
    public virtual CaracterMedio CaracterMedio { get; set; }
    public virtual ClasificacionMedio ClasificacionMedio { get; set; }
    public virtual TitularidadMedio TitularidadMedio { get; set; }
    public virtual Municipio Municipio { get; set; }
}
