﻿using DGPCE.Sigemad.Application.Dtos.Archivos;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;

namespace DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
public class ActivacionPlanEmergenciaDto
{
    public int Id { get; set; }
    public TipoPlanDto? TipoPlan { get; set; }
    public string? TipoPlanPersonalizado { get; set; }
    public PlanEmergenciaVm? PlanEmergencia { get; set; }
    public string? PlanEmergenciaPersonalizado { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string Autoridad { get; set; }
    public string? Observaciones { get; set; }
    public ArchivoDto? Archivo { get; set; }
}
