namespace DGPCE.Sigemad.Application.Dtos.Direcciones;
public class CreateOrUpdateDireccionDto
{
    public int? Id { get; set; }
    public int IdTipoDireccionEmergencia { get; set; }
    public string AutoridadQueDirige { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
}
