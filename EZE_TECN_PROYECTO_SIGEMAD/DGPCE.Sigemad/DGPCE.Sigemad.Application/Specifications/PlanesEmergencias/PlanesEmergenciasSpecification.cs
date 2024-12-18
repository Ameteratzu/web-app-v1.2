
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.PlanesEmergencias;
public class PlanesEmergenciasSpecification : BaseSpecification<PlanEmergencia>
{
    public PlanesEmergenciasSpecification(PlanesEmegenciasParams request)
        : base(PlanEmergencia =>
        (string.IsNullOrEmpty(request.Codigo) || PlanEmergencia.Codigo.Contains(request.Codigo)) &&
         (string.IsNullOrEmpty(request.Descripcion) || PlanEmergencia.Codigo.Contains(request.Descripcion)) &&
        (!request.Id.HasValue || PlanEmergencia.Id == request.Id) &&
        (!request.IdCcaa.HasValue || PlanEmergencia.IdCcaa == request.IdCcaa) &&
        (!request.IdProvincia.HasValue || PlanEmergencia.IdProvincia == request.IdProvincia) &&
        (!request.IdMunicipio.HasValue || PlanEmergencia.IdMunicipio == request.IdMunicipio) &&
        (!request.IdTipoPlan.HasValue || PlanEmergencia.IdTipoPlan == request.IdTipoPlan) &&
        (!request.IdTipoRiesgo.HasValue || PlanEmergencia.IdTipoRiesgo == request.IdTipoRiesgo) &&
        (!request.IdAmbitoPlan.HasValue || PlanEmergencia.IdAmbitoPlan == request.IdAmbitoPlan)

        )
    {
        AddOrderBy(p => p.Descripcion ?? string.Empty);
    }
}
