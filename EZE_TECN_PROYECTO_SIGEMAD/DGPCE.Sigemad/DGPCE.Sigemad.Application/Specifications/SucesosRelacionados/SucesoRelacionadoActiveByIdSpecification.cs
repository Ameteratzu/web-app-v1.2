using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
public class SucesoRelacionadoActiveByIdSpecification : BaseSpecification<SucesoRelacionado>
{
    public SucesoRelacionadoActiveByIdSpecification(int id) 
        : base(b => b.Id == id && b.Borrado == false)
    {
    }
}
