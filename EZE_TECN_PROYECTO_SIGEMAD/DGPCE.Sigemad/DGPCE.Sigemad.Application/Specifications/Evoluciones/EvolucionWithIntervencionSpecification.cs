using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionWithIntervencionSpecification : BaseSpecification<Evolucion>
{
    public EvolucionWithIntervencionSpecification(EvolucionSpecificationParams @params)
        : base(e =>
        (!@params.Id.HasValue || e.Id == @params.Id.Value) &&
        (!@params.IdSuceso.HasValue || e.IdSuceso == @params.IdSuceso.Value) &&
        e.Borrado == false)
    {
        AddInclude(e => e.IntervencionMedios);
        AddInclude("IntervencionMedios.DetalleIntervencionMedios");
    }
}
