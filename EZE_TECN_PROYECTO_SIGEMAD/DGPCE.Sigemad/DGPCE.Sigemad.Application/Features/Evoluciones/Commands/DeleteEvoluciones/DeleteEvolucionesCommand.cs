using MediatR;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvoluciones;
public class DeleteEvolucionesCommand : IRequest
{
    public int Id { get; set; }

}
