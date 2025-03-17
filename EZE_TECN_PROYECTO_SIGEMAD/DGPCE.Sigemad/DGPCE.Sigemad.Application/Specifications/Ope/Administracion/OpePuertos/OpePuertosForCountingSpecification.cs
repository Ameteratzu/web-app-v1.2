using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;

public class OpePuertosForCountingSpecification : BaseSpecification<OpePuerto>
{
    public OpePuertosForCountingSpecification(OpePuertosSpecificationParams request)
        : base(opePuerto =>
        (string.IsNullOrEmpty(request.Nombre) || opePuerto.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opePuerto.Id == request.Id) &&
        opePuerto.Borrado != true
        )
    {

    }
}
