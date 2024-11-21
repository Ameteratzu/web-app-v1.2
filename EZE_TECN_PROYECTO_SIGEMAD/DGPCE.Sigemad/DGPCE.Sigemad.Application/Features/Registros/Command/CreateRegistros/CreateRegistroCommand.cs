using MediatR;


namespace DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
public class CreateRegistroCommand : IRequest<CreateRegistroResponse>
{
    public int IdIncendio { get; set; }
    public int IdEvolucion { get; set; }
    public DateTime? FechaHoraEvolucion { get; set; }
    public int? IdEntradaSalida { get; set; }
    public int? IdMedio { get; set; }
}
