using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class CreateOpeDatoFronteraCommand : IRequest<CreateOpeDatoFronteraResponse>
{
    public int IdOpeFrontera { get; set; }
    public DateTime FechaHoraInicioIntervalo { get; set; }
    public DateTime FechaHoraFinIntervalo { get; set; }
    public int NumeroVehiculos { get; set; }
    public string Afluencia { get; set; } = null!;

}
