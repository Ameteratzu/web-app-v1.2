namespace DGPCE.Sigemad.Domain.Modelos;

public class Suceso
{
    public int Id { get; set; }

    public int IdTipo { get; set; }

    public virtual TipoSuceso TipoSuceso { get; set; } = null!;
}
