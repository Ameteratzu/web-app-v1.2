using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CoordinacionesPma.Commands.CreateOrUpdateCoordinacionPma;
public class CreateOrUpdateCoordinacionPmaCommand: IRequest<CreateOrUpdateCoordinacionPmaResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<CreateOrUpdateCoordinacionPmaDto> Coordinaciones { get; set; } = new();
}
