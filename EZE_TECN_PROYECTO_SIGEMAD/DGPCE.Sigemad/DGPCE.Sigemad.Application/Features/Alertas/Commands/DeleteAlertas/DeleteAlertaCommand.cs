using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Alertas.Commands.DeleteAlertas
{
    public class DeleteAlertaCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
