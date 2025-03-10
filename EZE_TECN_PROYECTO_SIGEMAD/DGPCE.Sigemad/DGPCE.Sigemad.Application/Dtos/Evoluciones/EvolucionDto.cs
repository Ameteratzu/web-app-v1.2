using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;

namespace DGPCE.Sigemad.Application.Dtos.Evoluciones;
public class EvolucionDto: BaseDto<int>
{
    public int IdSuceso { get; set; }

    public RegistroEvolucionDto? Registro { get; set; }
    public DatoPrincipalEvolucionDto? DatoPrincipal { get; set; }
    public ParametroEvolucionDto? Parametro { get; set; }
    public List<AreaAfectadaDto> AreaAfectadas { get; set; } = new();
    public List<ImpactoEvolucionDto> Impactos { get; set; } = new();
    public List<IntervencionMedioDto> IntervencionMedios { get; set; } = new();
}

