using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Features.Incendios.Vms
{
    public class IncendioVm
    {
        public string Denominacion { get; set; } = null!;

        public decimal? UtmX { get; set; }

        public decimal? UtmY { get; set; }

        public int? Huso { get; set; }

        public Geometry? GeoPosicion { get; set; }
        public string? Contenido { get; set; }

        public string? Comentarios { get; set; }

        public int IdClaseSuceso { get; set; }

        public bool CoordenadasReales { get; set; }
        public DateTime FechaInicio { get; set; }
        public string? RutaMapaRiesgo { get; set; }


        public virtual ClaseSuceso ClaseSuceso { get; set; } = null!;

        public virtual MunicipioSinIdProvinciaVm Municipio { get; set; } = null!;

        public virtual ProvinciaSinMunicipiosVm Provincia { get; set; } = null!;

        public virtual Suceso Suceso { get; set; } = null!;
        public virtual Territorio Territorio { get; set; } = null!;

        public virtual EstadoSuceso EstadoSuceso { get; set; } = null!;
        public virtual Pais Pais { get; set; } = null!;
    }
}
