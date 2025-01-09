using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
public class ActuacionRelevanteDGPCESpecification : BaseSpecification<ActuacionRelevanteDGPCE>
{
    public ActuacionRelevanteDGPCESpecification(int id)
      : base(d => d.Id == id && d.Borrado == false)
    {
        AddInclude(d => d.EmergenciaNacional);
    }
}
