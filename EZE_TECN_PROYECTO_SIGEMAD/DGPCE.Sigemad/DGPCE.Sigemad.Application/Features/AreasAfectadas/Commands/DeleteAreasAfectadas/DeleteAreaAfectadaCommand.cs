using MediatR; 
namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.DeleteAreasAfectadas;
public class DeleteAreaAfectadaCommand : IRequest
{
    public int Id { get; set; }
}

