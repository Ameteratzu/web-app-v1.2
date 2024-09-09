namespace DGPCE.Sigemad.Domain.Modelos;

public class Ccaa
{
    public Ccaa() { }

    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
