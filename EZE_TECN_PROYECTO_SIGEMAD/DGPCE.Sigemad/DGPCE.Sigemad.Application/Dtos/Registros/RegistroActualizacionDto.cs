namespace DGPCE.Sigemad.Application.Dtos.Registros;
public class RegistroActualizacionDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public string Registro { get; set; } // Entrada/Salida/Interna
    public string Origen { get; set; }
    public string TipoRegistro { get; set; } // Ejemplo: Datos de evolución, Otra información, etc.
    public string Tecnico { get; set; } // Técnico que realizó la acción

    public bool EsUltimoRegistro { get; set; } // Técnico que realizó la acción

}
