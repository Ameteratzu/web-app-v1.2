using MediatR;

namespace DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
public class CreateRegistroCommand
{
    public DateTime? FechaHoraEvolucion { get; set; }
    public int? IdEntradaSalida { get; set; }
    public int? IdMedio { get; set; }
}
