using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;

public class Evolucion : BaseDomainModel<int>
{
    public int IdSuceso { get; set; }
    public virtual Suceso Suceso { get; set; }
    public virtual Registro Registro { get; set; }
    
    public virtual DatoPrincipal DatoPrincipal { get; set; }
    public virtual Parametro Parametro { get; set; }

    public List<AreaAfectada> AreaAfectadas { get; set; } = new();
    public List<ImpactoEvolucion> Impactos { get; set; } = new();

}
