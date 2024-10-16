using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;

public class UpdateIncendioCommand: IRequest
{
    public int Id { get; set; }
    public int IdTerritorio { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Denominacion { get; set; }
    public DateTime FechaInicio { get; set; }
    public int IdTipoSuceso { get; set; }
    public int IdClaseSuceso { get; set; }
    public int IdEstadoSuceso { get; set; }
    public string Comentarios { get; set; }
    public string? RutaMapaRiesgo { get; set; }
    public Geometry GeoPosicion { get; set; }
}
