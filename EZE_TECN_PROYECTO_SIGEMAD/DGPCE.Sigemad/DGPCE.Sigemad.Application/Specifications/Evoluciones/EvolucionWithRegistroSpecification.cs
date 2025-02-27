using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionWithRegistroSpecification : BaseSpecification<Evolucion>
{
    public EvolucionWithRegistroSpecification(EvolucionSpecificationParams @params)
        : base(e =>
        (!@params.Id.HasValue || e.Id == @params.Id.Value) &&
        (!@params.IdSuceso.HasValue || e.IdSuceso == @params.IdSuceso.Value) &&
        e.Borrado == false)
    {
        AddInclude(e => e.Registro);
        AddInclude(e => e.Registro.ProcedenciaDestinos);
        AddInclude(e => e.DatoPrincipal);
        //AddInclude(e => e.Parametro);
        AddInclude("IntervencionMedios.DetalleIntervencionMedios");
    }
}
