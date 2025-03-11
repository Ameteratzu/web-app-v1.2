namespace DGPCE.Sigemad.Domain.Modelos.Ope;

public class OpePeriodoTipo
{
    public OpePeriodoTipo() { 
    }
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Borrado { get; set; }
}
