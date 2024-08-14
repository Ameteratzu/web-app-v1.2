using DGPCE.Sigemad.Application.Specifications.EstadosAlertas;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Alertas
{
    public class EstadosAlertasForCountingSpecification : BaseSpecification<EstadoAlerta>
    {
        public EstadosAlertasForCountingSpecification(EstadosAlertasSpecificationParams alertasParams)
            : base(
                    x =>
                     (string.IsNullOrEmpty(alertasParams.Search) || x.Descripcion!.Contains(alertasParams.Search)) 
                  )
        {
        }
    }
}
