using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class SucesoRelacionado :BaseDomainModel<int>
{
    public SucesoRelacionado()
    {     
    }

    public int IdSucesoPrincipal { get; set; }
    public int IdSucesoAsociado { get; set; }
    public string Observaciones { get; set; }
}
