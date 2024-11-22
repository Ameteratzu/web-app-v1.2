using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
public class CreateAreaAfectadaCommand : IRequest<CreateAreaAfectadaResponse>
{
    public int? IdEvolucion { get; set; }
    public int IdIncendio { get; set; }
    public List<CreateAreaAfectadaDto> AreasAfectadas { get; set; } = new();

}
