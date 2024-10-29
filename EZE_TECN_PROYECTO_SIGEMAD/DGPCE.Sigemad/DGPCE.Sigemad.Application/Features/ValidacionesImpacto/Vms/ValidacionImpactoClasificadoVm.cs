namespace DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
public class ValidacionImpactoClasificadoVm
{
    public int Id { get; set; }
    public int IdImpactoClasificado { get; set; }
    public string Campo { get; set; }
    public bool EsObligatorio { get; set; }
}
