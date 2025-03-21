using System;
using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeDatosFronteras;

public class OpeDatosFronterasSpecification : BaseSpecification<OpeDatoFrontera>
{
    public OpeDatosFronterasSpecification(OpeDatosFronterasSpecificationParams request)
        : base(opeDatoFrontera =>
        (!request.Id.HasValue || opeDatoFrontera.Id == request.Id) &&
       (!request.FechaHoraInicioIntervalo.HasValue || DateOnly.FromDateTime(opeDatoFrontera.FechaHoraFinIntervalo) >= request.FechaHoraInicioIntervalo) &&
            (!request.FechaHoraFinIntervalo.HasValue || DateOnly.FromDateTime(opeDatoFrontera.FechaHoraFinIntervalo) <= request.FechaHoraFinIntervalo) &&
        opeDatoFrontera.Borrado != true
        )
    {
        AddInclude(i => i.OpeFrontera);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaHoraInicioIntervalo);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaHoraInicioIntervalo);
                    break;
                case "denominacionasc":
                    //AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    //AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    AddOrderBy(i => i.FechaHoraInicioIntervalo); // Orden por defecto
                    break;
            }
        }
    }
}
