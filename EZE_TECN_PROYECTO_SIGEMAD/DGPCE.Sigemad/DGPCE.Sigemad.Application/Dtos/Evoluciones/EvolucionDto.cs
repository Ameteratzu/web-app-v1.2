using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.Evoluciones;
public class EvolucionDto
{
    public int Id { get; set; }
    public int IdIncendio { get; set; }

    public RegistroEvolucionDto? Registro { get; set; }
    public DatoPrincipalEvolucionDto? DatoPrincipal { get; set; }
    public ParametroEvolucionDto? Parametro { get; set; }
    public List<AreaAfectadaDto>? AreaAfectadas { get; set; }
    public List<ImpactoEvolucionDto>? Impactos { get; set; }
}

