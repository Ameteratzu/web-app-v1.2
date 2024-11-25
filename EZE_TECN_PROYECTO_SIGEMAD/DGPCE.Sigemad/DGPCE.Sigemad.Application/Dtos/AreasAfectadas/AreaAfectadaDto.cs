using DGPCE.Sigemad.Application.Dtos.EntidadesMenor;
using DGPCE.Sigemad.Application.Dtos.Municipios;
using DGPCE.Sigemad.Application.Dtos.Provincias;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
public class AreaAfectadaDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public ProvinciaDto Provincia { get; set; }
    public MunicipioDto Municipio { get; set; }
    public EntidadMenorDto? EntidadMenor { get; set; }
    public string Observaciones { get; set; }
    public Geometry GeoPosicion { get; set; }
}
