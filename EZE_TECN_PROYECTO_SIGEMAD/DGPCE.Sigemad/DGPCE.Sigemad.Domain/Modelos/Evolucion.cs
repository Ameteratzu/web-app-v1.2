﻿using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;

public class Evolucion : BaseDomainModel<int>
{
    public int IdIncendio { get; set; }

    //public int IdRegistro { get; set; }

    //public int IdDatoPrincipal { get; set; }

    //public int IdParametro { get; set; }

    public virtual Incendio Incendio { get; set; }

    public virtual Registro Registro { get; set; }
    public virtual DatoPrincipal DatoPrincipal { get; set; }
    public virtual Parametro Parametro { get; set; }

    public ICollection<RegistroProcedenciaDestino>? RegistroProcedenciasDestinos { get; set; } = null;

}
