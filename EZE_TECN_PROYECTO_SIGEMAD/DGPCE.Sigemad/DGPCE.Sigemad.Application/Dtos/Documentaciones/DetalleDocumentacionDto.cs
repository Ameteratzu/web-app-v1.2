using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.Documentaciones;
public class DetalleDocumentacionDto
{
    public int? Id { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public int IdTipoDocumento { get; set; }
    public string Descripcion { get; set; }
    public FileDto? Archivo { get; set; }

    public List<int>? IdsProcedenciasDestinos { get; set; } = new();

}