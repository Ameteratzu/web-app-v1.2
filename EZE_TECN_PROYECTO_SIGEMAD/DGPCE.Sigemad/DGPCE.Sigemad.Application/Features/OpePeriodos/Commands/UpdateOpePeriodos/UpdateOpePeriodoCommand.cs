using DGPCE.Sigemad.Domain.Enums;
using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.OpePeriodos.Commands.UpdateOpePeriodos;

public class UpdateOpePeriodoCommand : IRequest
{
    public int Id { get; set; }
 
    public string Denominacion { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

}
