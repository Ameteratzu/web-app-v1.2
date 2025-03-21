using DGPCE.Sigemad.Domain.Enums;
using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Commands.UpdateOpePuertos;

public class UpdateOpePuertoCommand : IRequest
{
    public int Id { get; set; }

    public string Nombre { get; set; }
    public int IdOpeFase { get; set; }
    public int IdPais { get; set; }
    public int IdCcaa { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string CoordenadaUTM_Y { get; set; }
    public string CoordenadaUTM_X { get; set; }
    public DateTime FechaValidezDesde { get; set; }
    public DateTime FechaValidezHasta { get; set; }
    public int Capacidad { get; set; }

}
