using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Domain.Modelos;
public class OpeDatoFrontera : BaseDomainModel<int>
{
    public int IdOpeFrontera { get; set; }
    public DateTime FechaHoraInicioIntervalo { get; set; }
    public DateTime FechaHoraFinIntervalo { get; set; }
    public string NumeroVehiculos { get; set; }
    public string Afluencia { get; set; }

  

    public virtual OpeFronteraConListaDatosFrontera OpeFronteraConListaDatosFrontera { get; set; }
}
