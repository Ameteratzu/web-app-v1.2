
using MediatR;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones
{
    public class CreateEvolucionCommand : IRequest<CreateEvolucionResponse>
    {
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
        public ICollection<int>? EvolucionProcedenciaDestinos { get; set; }
    }
}
