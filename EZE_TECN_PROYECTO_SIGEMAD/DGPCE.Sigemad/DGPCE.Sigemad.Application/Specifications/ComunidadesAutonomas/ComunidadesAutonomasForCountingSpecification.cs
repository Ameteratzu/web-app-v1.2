using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.ComunidadesAutonomas
{
    public class ComunidadesAutonomasForCountingSpecification : BaseSpecification<ComunidadAutonoma>
    {

        public ComunidadesAutonomasForCountingSpecification(ComunidadesAutonomasSpecificationParams comunidadesAutonomasParams)
               : base(
                    x =>
                     (string.IsNullOrEmpty(comunidadesAutonomasParams.Search) || x.Descripcion!.Contains(comunidadesAutonomasParams.Search))
                  )
        {
        }

    }
}
