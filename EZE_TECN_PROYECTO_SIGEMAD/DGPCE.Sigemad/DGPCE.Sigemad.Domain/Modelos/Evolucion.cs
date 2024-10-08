using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Domain.Modelos
{
    public class Evolucion : BaseDomainModel
    {
        public int IdIncendio { get; set; }
        public DateTime FechaHoraEvolucion { get; set; }
        public int IdEntradaSalida { get; set; }
        public int IdMedio { get; set; }
        public int? IdProcedenciaDestino { get; set; }
       
        public Guid  IdTecnico { get; set; }
        public bool Resumen { get; set; }
        public string? Observaciones { get; set; }
        public string? Prevision { get; set; }
        public string Estado { get; set; }
        public decimal? SuperficieAfectadaHectarea { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int IdProvinciaAfectada { get; set; }
        public int IdMunicipioAfectado { get; set; }
        public Geometry? GeoPosicionAreaAfectada { get; set; }
      

        public virtual Municipio Municipio { get; set; } = null!;

        public virtual Provincia Provincia { get; set; } = null!;

        public virtual Medio Medio { get; set; } = null!;

        public virtual ProcedenciaDestino ProcedenciaDestino { get; set; } = null!;

        public virtual EntradaSalida EntradaSalida { get; set; } = null!;

        public virtual ApplicationUser Tecnico { get; set; } = null!;

        public virtual Incendio Incendio { get; set; } = null!;

    }
}
