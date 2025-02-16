using DGPCE.Sigemad.Application.Dtos.Direcciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
public class CreateOrUpdateDireccionCommand: IRequest<CreateOrUpdateDireccionResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<CreateOrUpdateDireccionDto> Direcciones { get; set; } = new();
}
