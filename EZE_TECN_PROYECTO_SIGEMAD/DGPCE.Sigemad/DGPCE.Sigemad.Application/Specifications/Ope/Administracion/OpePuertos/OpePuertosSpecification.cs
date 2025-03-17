using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;

public class OpePuertosSpecification : BaseSpecification<OpePuerto>
{
    public OpePuertosSpecification(OpePuertosSpecificationParams request)
        : base(opePuerto =>
        (string.IsNullOrEmpty(request.Nombre) || opePuerto.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opePuerto.Id == request.Id) &&
        opePuerto.Borrado != true
        )
    {
        AddInclude(i => i.OpeFase);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaValidezDesde);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaValidezDesde);
                    break;
                case "denominacionasc":
                    AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    AddOrderBy(i => i.FechaValidezDesde); // Orden por defecto
                    break;
            }
        }
    }
}
