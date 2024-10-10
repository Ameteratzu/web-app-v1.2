using DGPCE.Sigemad.Application.Features.ApplicationUsers.Vms;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Vms
{
    public class EvolucionVm
    {
        public int Id { get; set; }
        public DateTime FechaHoraEvolucion { get; set; }
        public bool Resumen { get; set; }
        public string? Observaciones { get; set; }
        public string? Prevision { get; set; }
        public decimal? SuperficieAfectadaHectarea { get; set; }
        public DateTime? FechaFinal { get; set; }
        public Geometry? GeoPosicionAreaAfectada { get; set; }

        public virtual MunicipioSinIdProvinciaVm Municipio { get; set; } = null!;

        public virtual ProvinciaSinMunicipiosConIdComunidadVm Provincia { get; set; } = null!;

        public virtual Medio Medio { get; set; } = null!;

        public virtual ProcedenciaDestino ProcedenciaDestino { get; set; } = null!;

        public virtual EntradaSalida EntradaSalida { get; set; } = null!;

        public virtual ApplicationUserVm Tecnico { get; set; } = null!;

        public virtual EstadoEvolucion EstadoEvolucion { get; set; } = null!;
        public virtual EntidadMenor EntidadMenor { get; set; } = null!;
    }
}
