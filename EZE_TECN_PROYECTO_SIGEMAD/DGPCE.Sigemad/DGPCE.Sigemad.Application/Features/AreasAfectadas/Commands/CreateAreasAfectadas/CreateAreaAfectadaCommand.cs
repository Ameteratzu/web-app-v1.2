using MediatR;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
public class CreateAreaAfectadaCommand : IRequest<CreateAreaAfectadaResponse>
{
    public int IdEvolucion { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public int IdEntidadMenor { get; set; }
    public Geometry? GeoPosicion { get; set; }
}
