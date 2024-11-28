using DGPCE.Sigemad.Application.Dtos.ProcedenciasDestinos;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.Evoluciones;
public class RegistroEvolucionDto
{
    public DateTime FechaHoraEvolucion { get; set; }
    public Medio Medio { get; set; }
    public EntradaSalida EntradaSalida { get; set; }

    public List<ProcedenciaDto> ProcedenciaDestinos { get; set; }
}
