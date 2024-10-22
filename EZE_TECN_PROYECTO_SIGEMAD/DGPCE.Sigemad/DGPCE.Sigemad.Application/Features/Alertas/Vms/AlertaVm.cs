using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Features.Alertas.Vms
{
    public class AlertaVm
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaAlerta { get; set; }

        public Guid EstadoAlertaId { get; set; }

        public virtual EstadoAlerta? EstadoAlerta { get; set; }

    }
}
