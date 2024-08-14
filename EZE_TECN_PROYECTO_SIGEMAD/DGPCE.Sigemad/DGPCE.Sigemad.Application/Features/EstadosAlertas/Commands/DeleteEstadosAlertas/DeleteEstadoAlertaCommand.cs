using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.DeleteAlertas
{
    public class DeleteEstadoAlertaCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
