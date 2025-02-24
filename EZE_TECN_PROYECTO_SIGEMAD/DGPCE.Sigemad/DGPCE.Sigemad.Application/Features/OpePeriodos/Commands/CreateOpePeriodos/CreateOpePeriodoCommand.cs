
using MediatR;

namespace DGPCE.Sigemad.Application.Features.OpePeriodos.Commands.CreateOpePeriodos;

public class CreateOpePeriodoCommand : IRequest<CreateOpePeriodoResponse>
{  
    public string Nombre { get; set; }
    public DateTime FechaInicioFaseSalida { get; set; }
    public DateTime FechaFinFaseSalida { get; set; }

    public DateTime FechaInicioFaseRetorno { get; set; }
    public DateTime FechaFinFaseRetorno { get; set; }

}
