namespace DGPCE.Sigemad.API.Models;

public class ManageDocumentacionesRequest
{
    public int? IdDocumento { get; set; }
    public int IdSuceso { get; set; }
    public List<DetalleDocumentacionRequest> Detalles { get; set; } = new();
}