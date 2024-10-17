
namespace DGPCE.Sigemad.Domain.Modelos;
public class EvolucionProcedenciaDestino
{

    public int Id { get; set; }
    public int IdEvolucion { get; set; }
    public int IdProcedenciaDestino { get; set; }

    public Evolucion Evolucion { get; set; }
    public ProcedenciaDestino ProcedenciaDestino { get; set; }
}
