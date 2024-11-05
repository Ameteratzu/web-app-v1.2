using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Paises;
public class PaisesSpecification : BaseSpecification<Pais>
{
    public PaisesSpecification(bool excluirNacional)
        : base(p => !excluirNacional || p.Id != (int)PaisesEnum.Espana)
    {

    }
}
