using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Paises;
public class PaisesSpecification : BaseSpecification<Pais>
{
    public PaisesSpecification(bool mostrarNacional)
        : base(p => (mostrarNacional && p.Id == (int)PaisesEnum.Espana) ||
                    (!mostrarNacional && p.Id != (int)PaisesEnum.Espana))
    {
        if (!mostrarNacional)
        AddOrderBy(i => i.Descripcion);
    }

}
