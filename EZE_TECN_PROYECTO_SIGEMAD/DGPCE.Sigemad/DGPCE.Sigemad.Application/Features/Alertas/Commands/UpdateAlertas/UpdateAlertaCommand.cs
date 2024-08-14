using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas
{
    public class UpdateAlertaCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public DateTime? FechaAlerta { get; set; }

        public Guid EstadoAlertaId { get; set; }
    }
}
