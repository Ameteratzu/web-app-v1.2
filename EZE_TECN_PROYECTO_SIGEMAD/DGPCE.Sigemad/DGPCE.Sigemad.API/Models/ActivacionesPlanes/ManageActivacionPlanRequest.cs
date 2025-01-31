namespace DGPCE.Sigemad.API.Models.ActivacionesPlanes;

public class ManageActivacionPlanRequest
{
    public int? IdActuacionRelevante { get; set; }
    public int IdSuceso { get; set; }
    public List<ActivacionPlanRequest> ActivacionPlanes { get; set; } = new();
}
