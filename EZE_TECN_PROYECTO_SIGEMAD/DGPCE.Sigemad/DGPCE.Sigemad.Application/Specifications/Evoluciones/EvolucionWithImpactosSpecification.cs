using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionWithImpactosSpecification : BaseSpecification<Evolucion>
{
    public EvolucionWithImpactosSpecification(EvolucionSpecificationParams @params)
        : base(e =>
        (!@params.Id.HasValue || e.Id == @params.Id.Value) &&
        (!@params.IdSuceso.HasValue || e.IdSuceso == @params.IdSuceso.Value) &&
        e.Borrado == false)
    {
        AddInclude(e => e.Impactos);
        AddInclude("Impactos.ImpactoClasificado");
        AddInclude("Impactos.TipoDanio");
    }
}
