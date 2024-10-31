using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.AreasAfectadas;
public class AreaAfectadaActiveByIdEvolucionSpecification : BaseSpecification<AreaAfectada>
{
    public AreaAfectadaActiveByIdEvolucionSpecification(int idEvolucion)
        : base(a => a.IdEvolucion == idEvolucion && a.Borrado == false)
    {
        AddOrderBy(a => a.FechaHora);
    }
}
