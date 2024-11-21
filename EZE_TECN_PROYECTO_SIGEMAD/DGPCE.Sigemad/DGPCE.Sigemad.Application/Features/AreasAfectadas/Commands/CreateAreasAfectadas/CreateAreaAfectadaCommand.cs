using DGPCE.Sigemad.Application.Features.AreasAfectadas.Dtos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
public class CreateAreaAfectadaCommand : IRequest<CreateAreaAfectadaResponse>
{
    public int? IdEvolucion { get; set; }
    public int IdIncendio { get; set; }
    public List<AreaAfectadaDto> AreasAfectadas { get; set; } = new();

}
