namespace DGPCE.Sigemad.Domain.Modelos;

using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;
using System;

public class DireccionCoordinacionEmergencia : BaseDomainModel<int>
{
    public int IdIncendio { get; set; }
    public int IdTipoDireccionEmergencia { get; set; }
    public string AutoridadQueDirige { get; set; } = null!;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public DateTime? FechaInicioCECOPI { get; set; }
    public DateTime? FechaFinCECOPI { get; set; }
    public int IdProvinciaCECOPI { get; set; }
    public int IdMunicipioCECOPI { get; set; }
    public string? LugarCECOPI { get; set; }
    public Geometry? GeoPosicionCECOPI { get; set; }
    public string? ObservacionesCECOPI { get; set; }

    public DateTime FechaInicioPMA { get; set; }
    public DateTime? FechaFinPMA { get; set; }
    public int IdProvinciaPMA { get; set; } 
    public int IdMunicipioPMA { get; set; }
    public string? LugarPMA { get; set; } 
    public Geometry? GeoPosicionPMA { get; set; }
    public string? ObservacionesPMA { get; set; }


    public virtual Incendio Incendio { get; set; } = null!;
    public virtual TipoDireccionEmergencia TipoDireccionEmergencia { get; set; } = null!;
    public virtual Provincia ProvinciaCECOPI { get; set; } = null!;
    public virtual Municipio MunicipioCECOPI { get; set; } = null!;
    public virtual Provincia ProvinciaPMA { get; set; } = null!;
    public virtual Municipio MunicipioPMA { get; set; } = null!;
    public virtual ActivacionPlanEmergencia ActivacionPlanEmergencia { get; set; } = null!;
}
