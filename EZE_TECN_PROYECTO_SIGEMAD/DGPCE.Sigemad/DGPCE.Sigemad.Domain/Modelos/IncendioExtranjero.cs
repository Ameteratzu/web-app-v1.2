namespace DGPCE.Sigemad.Domain.Modelos;
public class IncendioExtranjero
{
    public int IdIncendio { get; set; }
    public int IdPais { get; set; }
    public int? IdDistrito { get; set; }
    public int? IdEntidadMenor { get; set; }
    public string Ubicacion { get; set; }


    public virtual Incendio Incendio { get; set; } = null!;
    public virtual Pais Pais { get; set; } = null!;
    public virtual Distrito Distrito { get; set; } = null!;
    public virtual EntidadMenor EntidadMenor { get; set; } = null!;
}
