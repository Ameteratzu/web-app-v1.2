using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeFronteraConListaDatosFrontera : BaseDomainModel<int>
{
    public OpeFronteraConListaDatosFrontera()
    {
        Lista = new List<OpeDatoFrontera>();
    }

  
    public List<OpeDatoFrontera> Lista { get; set; }

    
}
