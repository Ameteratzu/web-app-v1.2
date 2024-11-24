using DGPCE.Sigemad.Application.Dtos.Direcciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
public class CreateOrUpdateDireccionCommand: IRequest<CreateOrUpdateDireccionResponse>
{
    public int? IdDireccionCoordinacionEmergencia { get; set; }
    public int IdIncendio { get; set; }
    public List<CreateOrUpdateDireccionDto> Direcciones { get; set; } = new();
}
