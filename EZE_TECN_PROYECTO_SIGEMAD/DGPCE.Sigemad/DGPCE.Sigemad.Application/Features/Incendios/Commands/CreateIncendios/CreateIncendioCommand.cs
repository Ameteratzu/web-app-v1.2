using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;

public class CreateIncendioCommand : IRequest<int>
{
    public int IdTerritorio { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Denominacion { get; set; }
    public DateTime FechaInicio { get; set; }
    public int IdTipoSuceso { get; set; }
    public int IdClaseSuceso { get; set; }
    public int IdPeligroInicial { get; set; }
    public string Comentarios { get; set; }
    public Geometry GeoPosicion { get; set; }
    //public string WktUbicacion { get; set; }  // Se recibirá en formato WKT
}
