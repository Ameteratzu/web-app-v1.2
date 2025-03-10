using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class RegistroActualizacionWithDetailsForCountingSpecification : BaseSpecification<RegistroActualizacion>
{
    public RegistroActualizacionWithDetailsForCountingSpecification(RegistroActualizacionSpecificationParams @params)
        : base(r =>
        r.Borrado == false &&
        (!@params.Id.HasValue || r.Id == @params.Id) &&
        (!@params.IdMinimo.HasValue || r.Id > @params.IdMinimo.Value) &&
        (!@params.IdSuceso.HasValue || r.IdSuceso == @params.IdSuceso.Value) &&
        (!@params.IdTipoRegistroActualizacion.HasValue || r.IdTipoRegistroActualizacion == @params.IdTipoRegistroActualizacion.Value))
    {
        AddOrderByDescending(r => r.FechaCreacion);
    }
}
