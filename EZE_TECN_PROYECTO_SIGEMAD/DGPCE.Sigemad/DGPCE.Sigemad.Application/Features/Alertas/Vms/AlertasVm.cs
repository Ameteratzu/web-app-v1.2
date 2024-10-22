using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Alertas.Vms
{
    public class AlertasVm
    {
        public Guid Id { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaAlerta { get; set; }

        public Guid EstadoAlertaId { get; set; }

        public virtual EstadoAlerta? EstadoAlerta { get; set; }

    }
}
