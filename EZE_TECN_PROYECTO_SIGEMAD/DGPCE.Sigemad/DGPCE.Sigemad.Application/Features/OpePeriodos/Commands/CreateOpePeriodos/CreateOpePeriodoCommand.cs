
using MediatR;

namespace DGPCE.Sigemad.Application.Features.OpePeriodos.Commands.CreateOpePeriodos;

public class CreateOpePeriodoCommand : IRequest<CreateOpePeriodoResponse>
{

    
    public string Denominacion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

}
