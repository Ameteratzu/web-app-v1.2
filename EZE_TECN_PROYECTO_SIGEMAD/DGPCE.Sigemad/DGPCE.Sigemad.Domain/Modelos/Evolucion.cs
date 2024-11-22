using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;

public class Evolucion : BaseDomainModel<int>
{
    public int IdIncendio { get; set; }
    public virtual Incendio Incendio { get; set; }

    public ICollection<AreaAfectada> AreaAfectadas { get; set; } = new List<AreaAfectada>();
    public ICollection<ImpactoEvolucion> Impactos { get; set; } = new List<ImpactoEvolucion>();

}
