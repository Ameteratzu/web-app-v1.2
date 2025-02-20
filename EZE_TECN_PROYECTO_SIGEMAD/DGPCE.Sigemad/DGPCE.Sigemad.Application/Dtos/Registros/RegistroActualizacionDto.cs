namespace DGPCE.Sigemad.Application.Dtos.Registros;
public class RegistroActualizacionDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public string TipoRegistro { get; set; } = string.Empty;
    public string Apartados { get; set; } = string.Empty;
    public string Tecnico { get; set; } = string.Empty;

    public bool EsUltimoRegistro { get; set; } // Técnico que realizó la acción

}
