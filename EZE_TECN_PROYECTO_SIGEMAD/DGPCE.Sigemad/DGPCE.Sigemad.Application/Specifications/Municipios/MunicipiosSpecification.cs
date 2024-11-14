using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Municipios;

public class MunicipiosSpecification : BaseSpecification<Municipio>
{
    public MunicipiosSpecification(MunicipiosSpecificationParams request)
     : base(Provincia =>
         (!request.Id.HasValue || Provincia.Id == request.Id) &&
        (!request.IdProvincia.HasValue || Provincia.IdProvincia == request.IdProvincia) &&
        (Provincia.Borrado == false))
    {
        AddOrderBy(i => i.Descripcion); // Ordenación por defecto
    }

}
