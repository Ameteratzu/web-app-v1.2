namespace DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
public class CreateDetalleOtraInformacionDto
{
    public int? Id { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdMedio { get; set; }
    public string Asunto { get; set; }
    public string Observaciones { get; set; }
    public List<int> IdsProcedenciasDestinos { get; set; }
}
