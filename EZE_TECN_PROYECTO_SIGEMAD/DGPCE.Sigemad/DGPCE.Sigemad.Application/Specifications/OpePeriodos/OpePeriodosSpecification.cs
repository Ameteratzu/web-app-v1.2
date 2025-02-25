using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Periodos;

public class OpePeriodosSpecification : BaseSpecification<OpePeriodo>
{
    public OpePeriodosSpecification(OpePeriodosSpecificationParams request)
        : base(opePeriodo =>
        (string.IsNullOrEmpty(request.Nombre) || opePeriodo.Nombre.Contains(request.Nombre )) &&
        (!request.Id.HasValue || opePeriodo.Id == request.Id) &&
        (opePeriodo.Borrado != true)
        )
    {
        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaInicioFaseSalida);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaInicioFaseSalida);
                    break;
                case "denominacionasc":
                    AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    AddOrderBy(i => i.FechaInicioFaseSalida); // Orden por defecto
                    break;
            }
        }
    }
}
