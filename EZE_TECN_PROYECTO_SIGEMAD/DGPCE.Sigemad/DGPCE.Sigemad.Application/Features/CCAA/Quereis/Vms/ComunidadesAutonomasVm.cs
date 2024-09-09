using DGPCE.Sigemad.Application.Features.Alertas.Queries.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms
{
    public class ComunidadesAutonomasVm 
    {
        public int id { get; set; }
        public string? descripcion { get; set; }
    }
}
