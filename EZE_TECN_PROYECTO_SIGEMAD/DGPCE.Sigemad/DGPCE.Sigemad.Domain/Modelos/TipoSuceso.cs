namespace DGPCE.Sigemad.Domain.Modelos;

public class TipoSuceso
{
    public TipoSuceso() { }
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;
    public virtual ICollection<Suceso> Sucesos { get; set; } = new List<Suceso>();
}
