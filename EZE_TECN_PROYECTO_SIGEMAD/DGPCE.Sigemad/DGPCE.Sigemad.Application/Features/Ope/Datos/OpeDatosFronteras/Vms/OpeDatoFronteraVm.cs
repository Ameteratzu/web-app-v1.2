using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Vms
{
    public class OpeDatoFronteraVm : BaseDomainModel<int>
    {
        public int IdOpeFrontera { get; set; }
        public DateTime FechaHoraInicioIntervalo { get; set; }
        public DateTime FechaHoraFinIntervalo { get; set; }
        public int NumeroVehiculos { get; set; }
        public string Afluencia { get; set; } = null!;

        public virtual OpeFrontera OpeFrontera { get; set; } = null!;
    }
}
