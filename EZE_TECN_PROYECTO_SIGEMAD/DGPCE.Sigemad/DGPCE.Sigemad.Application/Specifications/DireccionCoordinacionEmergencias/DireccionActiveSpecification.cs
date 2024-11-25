using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionActiveSpecification : BaseSpecification<Direccion>
{
    public DireccionActiveSpecification()
    {
        AddInclude(d => d.TipoDireccionEmergencia);
    }
}
