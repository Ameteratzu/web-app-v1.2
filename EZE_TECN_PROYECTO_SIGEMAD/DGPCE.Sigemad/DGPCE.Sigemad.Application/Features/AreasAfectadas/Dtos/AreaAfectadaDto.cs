using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Dtos;
public class AreaAfectadaDto
{
    public DateTime FechaHora { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public int IdEntidadMenor { get; set; }
    public string? Observacion { get; set; }
    public Geometry? GeoPosicion { get; set; }
}
