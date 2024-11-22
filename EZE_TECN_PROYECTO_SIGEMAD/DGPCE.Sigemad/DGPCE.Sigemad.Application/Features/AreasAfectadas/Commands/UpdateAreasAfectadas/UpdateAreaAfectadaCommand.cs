using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
public class UpdateAreaAfectadaCommand : IRequest<UpdateAreaAfectadaResponse>
{
    public int IdEvolucion { get; set; }
    public List<UpdateAreaAfectadaDto> AreasAfectadas { get; set; } = new();
}
