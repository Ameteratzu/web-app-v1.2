namespace DGPCE.Sigemad.Domain.Common;
public class BaseEntity
{
    public DateTime FechaCreacion { get; set; }
    public Guid? CreadoPor { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public Guid? ModificadoPor { get; set; }
    public DateTime? FechaEliminacion { get; set; }
    public Guid? EliminadoPor { get; set; }
    public bool Borrado { get; set; } = false;
}
