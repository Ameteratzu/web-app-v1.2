using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
public class ManageActivacionPlanEmergenciaDto
{
    public int? Id { get; set; }
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
