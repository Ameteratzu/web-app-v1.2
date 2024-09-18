namespace DGPCE.Sigemad.Domain.Modelos;

public class Suceso
{
    public int Id { get; set; }

    public int IdTipo { get; set; }

    public int? IdSigeMigracion { get; set; }

    public virtual TipoSuceso TipoSuceso { get; set; } = null!;

    public virtual ICollection<Incendio> Incendios { get; set; } = new List<Incendio>();
}
