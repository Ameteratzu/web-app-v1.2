using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CoordinacionCecopis.Commands.CreateCoordinacionCecopi;
public class CreateOrUpdateCoordinacionCecopiCommand : IRequest<CreateOrUpdateCoordinacionCecopiResponse>
{
    public int? IdDireccionCoordinacionEmergencia { get; set; }
    public int IdSuceso { get; set; }
    public List<CreateOrUpdateCoordinacionCecopiDto> Coordinaciones { get; set; } = new();
}
