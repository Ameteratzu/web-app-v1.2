using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
public class ManageConvocatoriaCECODCommand : IRequest<ManageConvocatoriaCECODResponse>
{
    public int? IdActuacionRelevante { get; set; }
    public int IdSuceso { get; set; }
    public List<ConvocatoriaCECODDto>? Detalles { get; set; } = new();
}
