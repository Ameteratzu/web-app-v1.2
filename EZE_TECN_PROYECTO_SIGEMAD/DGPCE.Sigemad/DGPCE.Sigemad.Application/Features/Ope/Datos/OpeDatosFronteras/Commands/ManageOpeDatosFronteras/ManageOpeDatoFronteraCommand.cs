using DGPCE.Sigemad.Application.Dtos.Ope.Datos;
using DGPCE.Sigemad.Application.Dtos.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class ManageOpeDatoFronteraCommand : IRequest<ManageOpeDatoFronteraResponse>
{
    public int IdOpeFrontera { get; set; }
    public List<CreateOpeDatoFronteraDto> Lista { get; set; }
}
