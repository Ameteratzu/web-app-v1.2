namespace DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
public class CreateOrUpdateCoordinacionCecopiDto
{
    public int? Id { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Lugar { get; set; }
    public string? Observaciones { get; set; }
}
