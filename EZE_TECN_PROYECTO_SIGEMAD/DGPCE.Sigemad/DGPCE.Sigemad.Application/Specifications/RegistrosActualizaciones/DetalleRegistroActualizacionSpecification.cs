using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class DetalleRegistroActualizacionSpecification : BaseSpecification<DetalleRegistroActualizacion>
{
    public DetalleRegistroActualizacionSpecification(DetalleRegistroActualizacionParams @params)
        : base(d =>
        d.Borrado == false &&
        d.RegistroActualizacion.IdSuceso == @params.IdSuceso
        )
    {
        AddOrderByDescending(r => r.FechaCreacion);

        ApplyPaging(@params);
    }
}
