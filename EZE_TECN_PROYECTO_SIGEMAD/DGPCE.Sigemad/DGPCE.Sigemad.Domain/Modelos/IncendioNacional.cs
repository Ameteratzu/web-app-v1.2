namespace DGPCE.Sigemad.Domain.Modelos;

public class IncendioNacional
{
    public int IdIncendio { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }

    public virtual Incendio Incendio { get; set; } = null!;
    public virtual Provincia Provincia { get; set; } = null!;
    public virtual Municipio Municipio { get; set; } = null!;
}
