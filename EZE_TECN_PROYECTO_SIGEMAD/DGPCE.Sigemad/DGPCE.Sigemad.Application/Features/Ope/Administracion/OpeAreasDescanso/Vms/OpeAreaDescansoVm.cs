﻿using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAreasDescanso.Vms
{
    public class OpeAreaDescansoVm : BaseDomainModel<int>
    {
        public string Nombre { get; set; } = null!;

        public int IdOpeAreaDescansoTipo { get; set; }
        public int IdCcaa { get; set; }
        public int IdProvincia { get; set; }
        public int IdMunicipio { get; set; }
        public string CarreteraPK { get; set; } = null!;
        public string CoordenadaUTM_X { get; set; } = null!;
        public string CoordenadaUTM_Y { get; set; } = null!;
        public int Capacidad { get; set; }

        public int IdOpeEstadoOcupacion { get; set; }

        public virtual OpeAreaDescansoTipo OpeAreaDescansoTipo { get; set; } = null!;
        public virtual Ccaa Ccaa { get; set; } = null!;
        public virtual Provincia Provincia { get; set; } = null!;
        public virtual Municipio Municipio { get; set; } = null!;
        public virtual OpeEstadoOcupacion OpeEstadoOcupacion { get; set; } = null!;
    }
}
