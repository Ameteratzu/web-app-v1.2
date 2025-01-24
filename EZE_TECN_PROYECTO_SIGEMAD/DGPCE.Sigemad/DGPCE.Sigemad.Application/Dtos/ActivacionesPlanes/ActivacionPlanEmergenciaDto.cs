using DGPCE.Sigemad.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
public  class ActivacionPlanEmergenciaDto
{
    public int Id { get; set; }
    public int? IdTipoPlan { get; set; }
    public string? TipoPlanPersonalizado { get; set; }
    public int? IdPlanEmergencia { get; set; }
    public string? PlanEmergenciaPersonalizado { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string Autoridad { get; set; }
    public string? Observaciones { get; set; }
    public Guid? IdArchivo { get; set; }
    public FileDto? Archivo { get; set; }
}
