using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;

public class OpeLineasMaritimasSpecification : BaseSpecification<OpeLineaMaritima>
{
    public OpeLineasMaritimasSpecification(OpeLineasMaritimasSpecificationParams request)
        : base(opeLineaMaritima =>
        (string.IsNullOrEmpty(request.Nombre) || opeLineaMaritima.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeLineaMaritima.Id == request.Id) &&
        opeLineaMaritima.Borrado != true
        )
    {

        AddInclude(i => i.OpePuertoOrigen);
        AddInclude(i => i.OpePuertoDestino);
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
