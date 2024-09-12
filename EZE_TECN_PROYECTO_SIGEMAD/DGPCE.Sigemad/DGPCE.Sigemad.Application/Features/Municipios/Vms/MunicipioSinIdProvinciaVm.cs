using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Municipios.Vms
{
    public class MunicipioSinIdProvinciaVm
    {
        public MunicipioSinIdProvinciaVm() { }

        public int Id { get; set; }

        public string Descripcion { get; set; } = null!;

        public int? UtmX { get; set; }

        public int? UtmY { get; set; }

        public string? Huso { get; set; }

        public Geometry? GeoPosicion { get; set; }

    }

}
