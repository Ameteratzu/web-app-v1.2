using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class Alerta : BaseDomainModel<int>
    {
        public Alerta()
        {
        }

        public string? Descripcion { get; set; }

        public DateTime? FechaAlerta { get; set; }

        public int EstadoAlertaId { get; set; }
        public EstadoAlerta? EstadoAlerta { get; set; }

    }

}
