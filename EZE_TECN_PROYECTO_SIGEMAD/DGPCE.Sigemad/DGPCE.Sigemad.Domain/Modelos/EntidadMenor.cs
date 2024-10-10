
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class EntidadMenor
    {
        public int Id { get; set; }

        public int? IdDistrito { get; set; }
        public string Descripcion { get; set; } = null!;

        public int? UtmX { get; set; }
        public int? UtmY { get; set; }
        public int? Huso { get; set; }

        public Geometry? GeoPosicion { get; set; }

        public virtual Distrito Distrito { get; set; } = null!;
    }
}
