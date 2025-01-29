using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCEActiveByIdSpecification : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCEActiveByIdSpecification(ActuacionRelevanteDGPCESpecificationParams request)
    : base(ActuacionRelevanteDGPCyE =>
      (!request.Id.HasValue || ActuacionRelevanteDGPCyE.Id == request.Id) &&
     (!request.IdSuceso.HasValue || ActuacionRelevanteDGPCyE.IdSuceso == request.IdSuceso) &&
     (ActuacionRelevanteDGPCyE.Borrado == false))
    {
        if (request.Id.HasValue)
        {
            AddInclude(d => d.EmergenciaNacional);

            AddInclude(d => d.ActivacionPlanEmergencias.Where(dir => !dir.Borrado));

            AddInclude(d => d.DeclaracionesZAGEP.Where(dir => !dir.Borrado));

            AddInclude(d => d.ActivacionSistemas.Where(dir => !dir.Borrado));

            AddInclude(d => d.ConvocatoriaCECOD.Where(dir => !dir.Borrado));

            AddInclude(d => d.NotificacionEmergencia.Where(dir => !dir.Borrado));
        }
    }

}
