using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionWithAreaAfectadaSpecification : BaseSpecification<Evolucion>
{
    public EvolucionWithAreaAfectadaSpecification(EvolucionSpecificationParams @params)
        : base(e =>
        (!@params.Id.HasValue || e.Id == @params.Id.Value) &&
        (!@params.IdSuceso.HasValue || e.IdSuceso == @params.IdSuceso.Value) &&
        e.Borrado == false)
    {
        AddInclude(e => e.AreaAfectadas);
        AddInclude("AreaAfectadas.Municipio");
        AddInclude("AreaAfectadas.Provincia");
        AddInclude("AreaAfectadas.EntidadMenor");
    }
}
