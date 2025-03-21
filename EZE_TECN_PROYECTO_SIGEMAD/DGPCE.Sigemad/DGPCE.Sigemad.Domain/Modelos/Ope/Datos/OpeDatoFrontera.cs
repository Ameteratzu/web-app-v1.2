using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoFrontera : BaseDomainModel<int>
{
    public int IdOpeFrontera { get; set; }
    public DateTime FechaHoraInicioIntervalo { get; set; }
    public DateTime FechaHoraFinIntervalo { get; set; }
    public int NumeroVehiculos { get; set; }
    public string Afluencia { get; set; }

    public virtual OpeFrontera OpeFrontera { get; set; }
}
