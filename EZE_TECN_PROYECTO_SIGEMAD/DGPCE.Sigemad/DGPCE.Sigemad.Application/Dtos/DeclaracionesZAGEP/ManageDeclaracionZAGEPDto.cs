
namespace DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
public class ManageDeclaracionZAGEPDto
{

    public int? Id { get; set; }
    public DateOnly FechaSolicitud { get; set; }
    public string Denominacion { get; set; }
    public string? Observaciones { get; set; } = null;
}
