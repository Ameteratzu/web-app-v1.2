using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;

public class Evolucion : BaseDomainModel<int>
{
    public int IdIncendio { get; set; }
    public virtual Incendio Incendio { get; set; }
    public virtual Registro Registro { get; set; }
    //public ICollection<RegistroProcedenciaDestino>? RegistroProcedenciasDestinos { get; set; } = null;
    
    public virtual DatoPrincipal DatoPrincipal { get; set; }
    public virtual Parametro Parametro { get; set; }

    public ICollection<AreaAfectada> AreaAfectadas { get; set; } = new List<AreaAfectada>();
    public ICollection<ImpactoEvolucion> Impactos { get; set; } = new List<ImpactoEvolucion>();

}
