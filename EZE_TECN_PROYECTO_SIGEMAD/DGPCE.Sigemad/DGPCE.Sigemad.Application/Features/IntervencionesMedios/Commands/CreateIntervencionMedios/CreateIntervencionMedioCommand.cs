using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.CreateIntervencionMedios;
public class CreateIntervencionMedioCommand : IRequest<CreateIntervencionMedioResponse>
{
    public int IdEvolucion { get; set; }
    public int IdTipoIntervencionMedio { get; set; }
    public int IdCaracterMedio { get; set; }
    public int IdClasificacionMedio { get; set; }
    public int IdTitularidadMedio { get; set; }
    public int IdMunicipio { get; set; }
    public int Cantidad { get; set; }
    public string Unidad { get; set; }
    public string Titular { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public string? Observaciones { get; set; }
}
