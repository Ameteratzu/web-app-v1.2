﻿using System.ComponentModel.DataAnnotations;

namespace DGPCE.Sigemad.API.Models.ActivacionesPlanes;

public class ActivacionPlanRequest
{
    public int? Id { get; set; }
    public int? IdTipoPlan { get; set; }
    public string? TipoPlanPersonalizado { get; set; }
    public int? IdPlanEmergencia { get; set; }
    public string? PlanEmergenciaPersonalizado { get; set; }

    [Required]
    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    [Required]
    [MaxLength(255)]
    public string Autoridad { get; set; }

    public string? Observaciones { get; set; }

    public IFormFile? Archivo { get; set; }

}
