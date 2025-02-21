using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Periodos;

public class OpePeriodosSpecification : BaseSpecification<OpePeriodo>
{
    public OpePeriodosSpecification(OpePeriodosSpecificationParams request)
        : base(opePeriodo =>
        (string.IsNullOrEmpty(request.Search) || opePeriodo.Denominacion.Contains(request.Search)) &&
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
                    AddOrderBy(i => i.FechaInicio);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaInicio);
                    break;
                case "denominacionasc":
                    AddOrderBy(i => i.Denominacion);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Denominacion);
                    break;
                default:
                    AddOrderBy(i => i.FechaInicio); // Orden por defecto
                    break;
            }
        }
    }
}
