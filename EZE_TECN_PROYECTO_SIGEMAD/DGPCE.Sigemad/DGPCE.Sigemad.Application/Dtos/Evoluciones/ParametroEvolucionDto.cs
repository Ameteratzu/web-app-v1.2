using DGPCE.Sigemad.Application.Features.Fases.Vms;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.Evoluciones;
public class ParametroEvolucionDto
{

    public DateTime? FechaFinal { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public PlanEmergenciaVm? PlanEmergencia { get; set; }

    public EstadoIncendio EstadoIncendio { get; set; }

    public FaseEmergenciaVm? FaseEmergencia { get; set; }
    public PlanSituacionVm? PlanSituacion { get; set; }
}
