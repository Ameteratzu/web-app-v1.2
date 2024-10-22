using DGPCE.Sigemad.Application.Features.Distritos.Vms;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
public class EntidadMenorVm
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;

    public int? UtmX { get; set; }
    public int? UtmY { get; set; }
    public int? Huso { get; set; }

    public Geometry? GeoPosicion { get; set; }

    public virtual DistritoVm Distrito { get; set; } = null!;
}
