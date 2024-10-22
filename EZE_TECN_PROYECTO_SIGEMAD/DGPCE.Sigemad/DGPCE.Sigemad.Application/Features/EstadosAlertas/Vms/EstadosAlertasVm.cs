using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Vms
{
    public class EstadosAlertasVm
    {
        public Guid Id { get; set; }
        public string? Descripcion { get; set; }
    }
}
