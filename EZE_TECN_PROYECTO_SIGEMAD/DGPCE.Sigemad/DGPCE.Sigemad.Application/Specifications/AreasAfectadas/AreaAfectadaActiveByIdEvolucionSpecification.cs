using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.AreasAfectadas;
public class AreaAfectadaActiveByIdEvolucionSpecification : BaseSpecification<AreaAfectada>
{
    public AreaAfectadaActiveByIdEvolucionSpecification(int idEvolucion)
        : base(a => a.IdEvolucion == idEvolucion && a.Borrado == false)
    {
        AddInclude(a => a.Provincia);
        AddInclude(a => a.Municipio);
        AddInclude(a => a.EntidadMenor);
        AddOrderBy(a => a.FechaHora);
    }
}
