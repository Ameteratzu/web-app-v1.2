namespace DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
public class ValidacionImpactoClasificadoVm
{
    public int Id { get; set; }
    public int IdImpactoClasificado { get; set; }
    public string Campo { get; set; }
    public string TipoCampo { get; set; }
    public bool EsObligatorio { get; set; }
    public string Label { get; set; } = string.Empty;
    public List<OptionVm> Options { get; set; } = new List<OptionVm>();
}

public class OptionVm
{
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}