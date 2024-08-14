using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Alertas
{
    public class AlertasForCountingSpecification : BaseSpecification<Alerta>
    {
        public AlertasForCountingSpecification(AlertasSpecificationParams alertasParams)
            : base(
                    x =>
                     (string.IsNullOrEmpty(alertasParams.Search) || x.Descripcion!.Contains(alertasParams.Search)) &&
                     (!alertasParams.IdEstado.HasValue || x.EstadoAlertaId == alertasParams.IdEstado) &&
                     (!alertasParams.FechaAlerta.HasValue || x.FechaAlerta == alertasParams.FechaAlerta)
                  )
        {
        }
    }
}
