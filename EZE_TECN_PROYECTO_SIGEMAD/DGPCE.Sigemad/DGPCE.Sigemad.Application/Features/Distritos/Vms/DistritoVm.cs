
namespace DGPCE.Sigemad.Application.Features.Distritos.Vms;
public class DistritoVm
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public int IdPais { get; set; }

    public string? CodigoOficial { get; set; }
}
