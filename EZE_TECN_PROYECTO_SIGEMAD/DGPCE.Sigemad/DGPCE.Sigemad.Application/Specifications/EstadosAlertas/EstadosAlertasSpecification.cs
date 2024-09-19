using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.EstadosAlertas
{
    public class EstadosAlertasSpecification : BaseSpecification<EstadoAlerta>
    {
        public EstadosAlertasSpecification(EstadosAlertasSpecificationParams estadosAlertasParams)
           : base(
                   x =>
                    (string.IsNullOrEmpty(estadosAlertasParams.Search) || x.Descripcion!.Contains(estadosAlertasParams.Search)) 
                 )
        {

            ApplyPaging(estadosAlertasParams.PageSize * (estadosAlertasParams.Page - 1), estadosAlertasParams.PageSize);

            if (!string.IsNullOrEmpty(estadosAlertasParams.Sort))
            {
                switch (estadosAlertasParams.Sort)
                {
                    case "descripcionAsc":
                        AddOrderBy(p => p.Descripcion!);
                        break;

                    case "descripcionDesc":
                        AddOrderByDescending(p => p.Descripcion!);
                        break;
    
                    default:
                        AddOrderBy(p => p.FechaCreacion!);
                        break;
                }
            }
        }
    }
}
