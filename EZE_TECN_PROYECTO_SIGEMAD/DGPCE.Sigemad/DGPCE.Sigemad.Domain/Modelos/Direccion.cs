using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Direccion : BaseDomainModel<int>
{
    public int IdDireccionCoordinacionEmergencia { get; set; }
    public virtual DireccionCoordinacionEmergencia DireccionCoordinacionEmergencia { get; set; }

    public int IdTipoDireccionEmergencia { get; set; }
    public virtual TipoDireccionEmergencia TipoDireccionEmergencia { get; set; }

    public string AutoridadQueDirige { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }

    
}
