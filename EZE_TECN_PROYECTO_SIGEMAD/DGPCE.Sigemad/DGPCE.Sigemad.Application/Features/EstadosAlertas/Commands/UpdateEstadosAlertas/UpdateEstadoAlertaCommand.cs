using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas
{
    public class UpdateEstadoAlertaCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

    }
}
