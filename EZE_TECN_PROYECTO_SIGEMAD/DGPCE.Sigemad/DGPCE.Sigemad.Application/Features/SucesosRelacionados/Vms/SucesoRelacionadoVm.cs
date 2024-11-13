namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
public class SucesoRelacionadoVm
{
    public int Id { get; set; }
    public int IdSucesoPrincipal { get; set; }
    public int IdSucesoAsociado { get; set; }
    public string Observaciones { get; set; }
    public DateTime? FechaCreacion { get; set; }    
    public DateTime? FechaModificacion { get; set; }    
}
