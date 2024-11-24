namespace DGPCE.Sigemad.Domain.Modelos;

using DGPCE.Sigemad.Domain.Common;

public class DireccionCoordinacionEmergencia : BaseDomainModel<int>
{
    public int IdIncendio { get; set; }
    public Incendio Incendio { get; set; }

    public List<Direccion> Direcciones { get; set; } = new();
    public List<CoordinacionCecopi> CoordinacionCecopis { get; set; } = new();
    public List<CoordinacionPMA> CoordinacionPMAs { get; set; } = new();
}
