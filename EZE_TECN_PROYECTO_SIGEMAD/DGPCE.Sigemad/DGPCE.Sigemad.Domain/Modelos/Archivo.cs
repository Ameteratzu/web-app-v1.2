namespace DGPCE.Sigemad.Domain.Modelos;

public class Archivo
{
    public Guid Id { get; set; }
    public string NombreOriginal { get; set; }
    public string RutaDeAlmacenamiento { get; set; }
    public string NombreUnico { get; set; }
    public string Tipo { get; set; }
    public string Extension { get; set; }
    public long PesoEnBytes { get; set; }
    public DateTime FechaCreacion { get; set; }
    public Guid? CreadoPor { get; set; }
}
