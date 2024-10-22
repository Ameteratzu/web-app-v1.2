using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Intervenciones;
public class IntervencionActiveByIdEvolucionSpecification : BaseSpecification<IntervencionMedio>
{
    public IntervencionActiveByIdEvolucionSpecification(int idEvolucion)
        : base(i => i.IdEvolucion == idEvolucion && i.Borrado == false)
    {
        AddInclude(i => i.TipoIntervencionMedio);
        AddInclude(i => i.CaracterMedio);
        AddInclude(i => i.TitularidadMedio);
    }
}
