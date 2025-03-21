using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.UpdateOpeDatosFronteras;

public class UpdateOpeDatoFronteraCommand : IRequest
{
    public int Id { get; set; }
    public int IdOpeFrontera { get; set; }
    public DateTime FechaHoraInicioIntervalo { get; set; }
    public DateTime FechaHoraFinIntervalo { get; set; }
    public int NumeroVehiculos { get; set; }
    public string Afluencia { get; set; } = null!;
}
