using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuntosControlCarreteras.Commands.DeleteOpePuntosControlCarreteras
{
    public class DeleteOpePuntoControlCarreteraCommand : IRequest
    {
        public int Id { get; set; }
    }
}
