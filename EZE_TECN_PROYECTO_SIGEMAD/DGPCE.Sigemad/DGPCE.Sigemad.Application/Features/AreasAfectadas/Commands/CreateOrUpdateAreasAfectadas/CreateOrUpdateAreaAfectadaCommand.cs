using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateOrUpdateAreasAfectadas;
public class CreateOrUpdateAreaAfectadaCommand : IRequest<CreateOrUpdateAreaAfectadaResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<CreateOrUpdateAreaAfectadaDto> AreasAfectadas { get; set; } = new();

}
