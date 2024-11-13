using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
internal class SucesosRelacionadosActiveByIdSucesoPrincipalSpecification : BaseSpecification<SucesoRelacionado>
{
    public SucesosRelacionadosActiveByIdSucesoPrincipalSpecification(int idSucesoPrincipal)
        : base(b => b.IdSucesoPrincipal == idSucesoPrincipal && b.Borrado == false)
    {
    }
}
