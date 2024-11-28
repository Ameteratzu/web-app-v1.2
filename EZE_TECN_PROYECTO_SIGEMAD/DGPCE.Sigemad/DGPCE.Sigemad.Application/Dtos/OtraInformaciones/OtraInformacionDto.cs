using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
public class OtraInformacionDto: BaseDto<int>
{
    public int IdIncendio { get; set; }

    public List<DetalleOtraInformacionDto> Lista { get; set; } = new();
}
