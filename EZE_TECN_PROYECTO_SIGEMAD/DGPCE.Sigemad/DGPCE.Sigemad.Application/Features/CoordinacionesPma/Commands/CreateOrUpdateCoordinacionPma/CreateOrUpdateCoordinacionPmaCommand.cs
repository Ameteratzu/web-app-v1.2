using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CoordinacionesPma.Commands.CreateOrUpdateCoordinacionPma;
public class CreateOrUpdateCoordinacionPmaCommand: IRequest<CreateOrUpdateCoordinacionPmaResponse>
{
    public int? IdDireccionCoordinacionEmergencia { get; set; }
    public int IdIncendio { get; set; }
    public List<CreateOrUpdateCoordinacionPmaDto> Coordinaciones { get; set; } = new();
}
