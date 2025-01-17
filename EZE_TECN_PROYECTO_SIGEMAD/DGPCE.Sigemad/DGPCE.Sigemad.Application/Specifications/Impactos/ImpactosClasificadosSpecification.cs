using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Impactos;
public class ImpactosClasificadosSpecification : BaseSpecification<ImpactoClasificado>
{
    public ImpactosClasificadosSpecification(ImpactosClasificadosSpecificationParams @params)
        : base(i =>
        (string.IsNullOrEmpty(@params.Tipo) || i.TipoImpacto.Contains(@params.Tipo)) &&
        (string.IsNullOrEmpty(@params.Grupo) || i.GrupoImpacto.Contains(@params.Grupo))
        )
    {
    }
}
