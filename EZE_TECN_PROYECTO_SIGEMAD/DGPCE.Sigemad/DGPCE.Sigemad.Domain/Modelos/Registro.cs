using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Registro : BaseDomainModel<int>
{
    public int IdEvolucion { get; set; }

    public DateTime? FechaHoraEvolucion { get; set; }
    public int? IdEntradaSalida { get; set; }
    public int? IdMedio { get; set; }

    public virtual Medio Medio { get; set; } = null!;
    public virtual EntradaSalida EntradaSalida { get; set; } = null!;
    public ICollection<RegistroProcedenciaDestino>? RegistrosProcedenciasDestinos { get; set; } = null;

}
