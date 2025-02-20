using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class RegistroActualizacionWithDetailsSpecification : BaseSpecification<RegistroActualizacion>
{
    public RegistroActualizacionWithDetailsSpecification(RegistroActualizacionSpecificationParams @params)
        : base(r =>
        r.Borrado == false &&
        (!@params.Id.HasValue || r.Id == @params.Id) &&
        (!@params.IdMinimo.HasValue || r.Id > @params.IdMinimo.Value) &&
        (!@params.IdSuceso.HasValue || r.IdSuceso == @params.IdSuceso.Value) &&
        (!@params.IdTipoRegistroActualizacion.HasValue || r.IdTipoRegistroActualizacion == @params.IdTipoRegistroActualizacion.Value))
    {
        AddInclude(r => r.TipoRegistroActualizacion);
        AddInclude(r => r.DetallesRegistro);
        AddInclude("DetallesRegistro.ApartadoRegistro");

        AddOrderByDescending(r => r.FechaCreacion);

        ApplyPaging(@params);
    }
}
