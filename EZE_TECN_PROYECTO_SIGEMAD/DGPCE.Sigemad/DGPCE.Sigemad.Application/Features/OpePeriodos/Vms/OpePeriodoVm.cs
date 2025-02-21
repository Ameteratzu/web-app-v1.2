
using DGPCE.Sigemad.Domain.Common;



namespace DGPCE.Sigemad.Application.Features.Periodos.Vms
{
    public class OpePeriodoVm : BaseDomainModel<int>
    {
        public string Denominacion { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
