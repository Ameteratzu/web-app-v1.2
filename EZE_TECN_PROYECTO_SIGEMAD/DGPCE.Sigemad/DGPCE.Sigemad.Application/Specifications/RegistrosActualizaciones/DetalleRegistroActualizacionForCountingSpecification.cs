using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class DetalleRegistroActualizacionForCountingSpecification : BaseSpecification<DetalleRegistroActualizacion>
{
    public DetalleRegistroActualizacionForCountingSpecification(DetalleRegistroActualizacionParams @params)
        : base(d =>
        d.Borrado == false &&
        d.RegistroActualizacion.IdSuceso == @params.IdSuceso
        )
    {
        AddOrderByDescending(r => r.FechaCreacion);
    }
}
