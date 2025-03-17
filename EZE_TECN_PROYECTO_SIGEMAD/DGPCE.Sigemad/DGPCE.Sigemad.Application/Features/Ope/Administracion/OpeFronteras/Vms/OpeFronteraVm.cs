﻿using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Vms
{
    public class OpeFronteraVm : BaseDomainModel<int>
    {
        public string Nombre { get; set; } = null!;
        public int IdCcaa { get; set; }
        public int IdProvincia { get; set; }
        public int IdMunicipio { get; set; }
        public string CarreteraPK { get; set; } = null!;
        public string CoordenadaUTM_X { get; set; } = null!;
        public string CoordenadaUTM_Y { get; set; } = null!;
        public int TransitoMedioVehiculos { get; set; }
        public int TransitoAltoVehiculos { get; set; }

        public virtual Ccaa Ccaa { get; set; } = null!;
        public virtual Provincia Provincia { get; set; } = null!;
        public virtual Municipio Municipio { get; set; } = null!;
    }
}
