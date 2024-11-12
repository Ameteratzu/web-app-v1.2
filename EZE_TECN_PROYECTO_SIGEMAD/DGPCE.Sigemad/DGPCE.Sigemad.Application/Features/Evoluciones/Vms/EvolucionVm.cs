

using DGPCE.Sigemad.Application.Features.EvolucionProcedenciaDestinos.Vms;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Vms
{
    public class EvolucionVm
    {
        public int Id { get; set; }
        public int IdIncendio { get; set; }
        public DateTime? FechaHoraEvolucion { get; set; }
        public int? IdEntradaSalida { get; set; }
        public int? IdMedio { get; set; }
        public int? IdTipoRegistro { get; set; }
        public string? Observaciones { get; set; }
        public string? Prevision { get; set; }
        public int? IdEstadoIncendio { get; set; }
        public string? PlanEmergenciaActivado { get; set; }
        public int? IdSituacionOperativa { get; set; }
        public decimal? SuperficieAfectadaHectarea { get; set; }
        public DateTime? FechaFinal { get; set; }

        public ICollection<EvolucionProcedenciaDestinoVm>? EvolucionProcedenciaDestinos { get; set; } = null;
    }
}
