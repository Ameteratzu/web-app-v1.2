using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
public class ManageDeclaracionesZAGEPCommand : IRequest<ManageDeclaracionZAGEPResponse>
{
    public int? IdActuacionRelevante { get; set; }
    public int IdSuceso { get; set; }
    public List<DeclaracionZAGEPDto>? Detalles { get; set; } = new();
}
