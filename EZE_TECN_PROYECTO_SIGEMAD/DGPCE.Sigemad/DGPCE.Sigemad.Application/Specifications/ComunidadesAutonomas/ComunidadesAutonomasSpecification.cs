using DGPCE.Sigemad.Application.Specifications.EstadosAlertas;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.ComunidadesAutonomas
{
    public class ComunidadesAutonomasSpecification : BaseSpecification<ComunidadAutonoma>
    {

        public ComunidadesAutonomasSpecification(ComunidadesAutonomasSpecificationParams comunidadesAutonomasParams)
        : base(
                x =>
                 (string.IsNullOrEmpty(comunidadesAutonomasParams.Search) || x.Descripcion!.Contains(comunidadesAutonomasParams.Search))
              )
        {

            ApplyPaging(comunidadesAutonomasParams.PageSize * (comunidadesAutonomasParams.PageIndex - 1), comunidadesAutonomasParams.PageSize);

            if (!string.IsNullOrEmpty(comunidadesAutonomasParams.Sort))
            {
                switch (comunidadesAutonomasParams.Sort)
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
