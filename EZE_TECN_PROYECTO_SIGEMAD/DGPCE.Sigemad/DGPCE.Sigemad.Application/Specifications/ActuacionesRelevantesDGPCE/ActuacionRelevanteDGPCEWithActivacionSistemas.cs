using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCEWithActivacionSistemas : BaseSpecification<ActuacionRelevanteDGPCE>
{  public ActuacionRelevanteDGPCEWithActivacionSistemas(ActuacionRelevanteDGPCESpecificationParams @params)
 : base(d =>
 (!@params.Id.HasValue || d.Id == @params.Id) &&
 (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
  d.Borrado == false)
    {
        AddInclude(d => d.ActivacionSistemas);
        AddInclude("ActivacionSistemas.TipoSistemaEmergencia");
        AddInclude("ActivacionSistemas.ModoActivacion");
        
    }
}

