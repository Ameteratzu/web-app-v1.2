using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas
{
    public class CreateEstadoAlertaCommand : IRequest<Guid>
    {
        public string? Descripcion { get; set; } = string.Empty;

    }
}
