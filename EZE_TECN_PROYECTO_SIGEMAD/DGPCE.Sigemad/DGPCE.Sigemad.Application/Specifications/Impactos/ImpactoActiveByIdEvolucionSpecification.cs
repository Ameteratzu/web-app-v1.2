using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Impactos;
public class ImpactoActiveByIdEvolucionSpecification : BaseSpecification<ImpactoEvolucion>
{
    public ImpactoActiveByIdEvolucionSpecification(int idEvolucion)
        : base(i => i.IdEvolucion == idEvolucion && i.Borrado == false)
    {
        AddInclude(i => i.ImpactoClasificado);
    }
}
