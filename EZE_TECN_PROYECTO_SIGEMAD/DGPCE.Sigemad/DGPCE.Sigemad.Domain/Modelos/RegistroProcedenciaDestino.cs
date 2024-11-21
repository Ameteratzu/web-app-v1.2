
namespace DGPCE.Sigemad.Domain.Modelos;
public class RegistroProcedenciaDestino
{

    public int Id { get; set; }
    public int IdRegistroEvolucion { get; set; }
    public int IdProcedenciaDestino { get; set; }

    public Registro Registro { get; set; }
    public ProcedenciaDestino ProcedenciaDestino { get; set; }
}
