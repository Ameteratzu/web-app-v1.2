using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class DetalleRegistroActualizacionSpecification : BaseSpecification<DetalleRegistroActualizacion>
{
    private static readonly List<int> idsApartadoNoPermitido = new List<int> { 1, 2, 3 };

    public DetalleRegistroActualizacionSpecification(DetalleRegistroActualizacionParams @params)
        : base(d =>
        d.Borrado == false &&
        d.RegistroActualizacion.IdSuceso == @params.IdSuceso &&
        !idsApartadoNoPermitido.Contains(d.IdApartadoRegistro)
        )
    {
        AddOrderByDescending(r => r.FechaCreacion);

        ApplyPaging(@params);
    }
}
