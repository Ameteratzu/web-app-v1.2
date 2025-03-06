using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
public class DetalleRegistroActualizacionForCountingSpecification : BaseSpecification<DetalleRegistroActualizacion>
{
    private static readonly List<int> idsApartadoNoPermitido = new List<int> { 1, 2, 3 };

    public DetalleRegistroActualizacionForCountingSpecification(DetalleRegistroActualizacionParams @params)
        : base(d =>
        d.Borrado == false &&
        d.RegistroActualizacion.IdSuceso == @params.IdSuceso &&
        !idsApartadoNoPermitido.Contains(d.IdApartadoRegistro)
        )
    {
        AddOrderByDescending(r => r.FechaCreacion);
    }
}
