using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class DetalleEvolucionSpecification: BaseSpecification<Evolucion>
{
    public DetalleEvolucionSpecification(int idIncendio)
        : base(e => e.IdIncendio == idIncendio)
    {
        AddInclude(e => e.EntradaSalida);
    }
}
