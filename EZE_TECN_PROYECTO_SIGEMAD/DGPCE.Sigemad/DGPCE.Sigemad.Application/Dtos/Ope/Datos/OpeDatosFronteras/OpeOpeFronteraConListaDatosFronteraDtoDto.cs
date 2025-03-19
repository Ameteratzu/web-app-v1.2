using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.Ope.Datos.OpeDatosFronteras;
public class OpeFronteraConListaDatosFronteraDto : BaseDto<int>
{
    public int IdOpeFrontera { get; set; }

    public List<OpeDatoFronteraDto> Lista { get; set; } = new();
}
