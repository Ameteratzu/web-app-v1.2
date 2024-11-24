using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class CoordinacionCecopi : BaseDomainModel<int>
{
    public virtual DireccionCoordinacionEmergencia DireccionCoordinacionEmergencia { get; set; }

    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }

    public int IdProvincia { get; set; }
    public Provincia Provincia { get; set; }

    public int IdMunicipio { get; set; }
    public Municipio Municipio { get; set; }
}
