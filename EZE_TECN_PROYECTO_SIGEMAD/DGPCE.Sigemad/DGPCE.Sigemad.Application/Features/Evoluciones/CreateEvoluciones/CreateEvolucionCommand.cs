
using MediatR;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.CreateEvolucion
{
        public class CreateEvolucionCommand : IRequest<CreateEvolucionResponse>
    {
        public int IdIncendio { get; set; }
        public DateTime FechaHoraEvolucion { get; set; }
        public int IdEntradaSalida { get; set; }
        public int IdMedio { get; set; }
        public int IdProcedenciaDestino { get; set; }

        public Guid IdTecnico { get; set; }
        public bool Resumen { get; set; }
        public string? Observaciones { get; set; }
        public string? Prevision { get; set; }
        public int IdEstadoEvolucion { get; set; }
        public decimal? SuperficieAfectadaHectarea { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int IdProvinciaAfectada { get; set; }
        public int IdMunicipioAfectado { get; set; }
        public Geometry? GeoPosicionAreaAfectada { get; set; }

    }
}
