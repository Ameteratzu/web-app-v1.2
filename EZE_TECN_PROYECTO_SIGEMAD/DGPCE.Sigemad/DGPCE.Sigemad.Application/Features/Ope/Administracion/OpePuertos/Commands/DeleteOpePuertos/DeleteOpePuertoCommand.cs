using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.DeleteOpePuertos
{
    public class DeleteOpePuertoCommand : IRequest
    {
        public int Id { get; set; }
    }
}
