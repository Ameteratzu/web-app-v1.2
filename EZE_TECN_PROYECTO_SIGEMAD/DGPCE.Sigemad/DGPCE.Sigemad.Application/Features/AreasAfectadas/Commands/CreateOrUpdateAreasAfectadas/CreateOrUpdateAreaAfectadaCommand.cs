using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
public class CreateOrUpdateAreaAfectadaCommand : IRequest<CreateOrUpdateAreaAfectadaResponse>
{
    public int? IdEvolucion { get; set; }
    public int IdIncendio { get; set; }
    public List<CreateOrUpdateAreaAfectadaDto> AreasAfectadas { get; set; } = new();

}
