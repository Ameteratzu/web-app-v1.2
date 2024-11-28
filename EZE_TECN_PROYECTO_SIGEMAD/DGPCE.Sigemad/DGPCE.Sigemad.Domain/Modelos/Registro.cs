using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Registro : BaseDomainModel<int>
{
    public Registro()
    {
        ProcedenciaDestinos = new();
    }

    public DateTime? FechaHoraEvolucion { get; set; }
    public int? IdEntradaSalida { get; set; }
    public int? IdMedio { get; set; }

    public virtual Evolucion Evolucion { get; set; } = null!;
    public virtual Medio Medio { get; set; } = null!;
    public virtual EntradaSalida EntradaSalida { get; set; } = null!;

    public virtual List<RegistroProcedenciaDestino> ProcedenciaDestinos { get; set; } = new();
}
