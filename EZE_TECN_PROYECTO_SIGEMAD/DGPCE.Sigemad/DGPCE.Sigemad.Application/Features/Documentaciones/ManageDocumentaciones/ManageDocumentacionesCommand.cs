
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;
public class ManageDocumentacionesCommand : IRequest<CreateOrUpdateDocumentacionResponse>
{
    public int? IdDocumento { get; set; }
    public int IdIncendio { get; set; }

    public List<DetalleDocumentacionDto> DetallesDocumentaciones { get; set; } = new();
}
