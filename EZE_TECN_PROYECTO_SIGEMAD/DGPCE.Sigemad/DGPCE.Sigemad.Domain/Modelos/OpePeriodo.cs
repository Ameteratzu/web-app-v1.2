using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class OpePeriodo : BaseDomainModel<int>
{
    public string Denominacion { get; set; } = null!;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
