using MediatR;

namespace DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas
{
    public class CreateAlertaCommand : IRequest<int>
    {
        public string? Descripcion { get; set; } = string.Empty;

        public DateTime? FechaAlerta { get; set; }

        public Guid EstadoAlertaId { get; set; }

    }
}
