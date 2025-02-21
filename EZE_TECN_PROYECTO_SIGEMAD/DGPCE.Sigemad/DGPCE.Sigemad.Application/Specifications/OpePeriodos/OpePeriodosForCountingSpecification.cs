using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Application.Specifications.Periodos;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.OpePeriodos;

public class OpePeriodosForCountingSpecification : BaseSpecification<OpePeriodo>
{
    public OpePeriodosForCountingSpecification(OpePeriodosSpecificationParams request)
        : base(opePeriodo =>
        (string.IsNullOrEmpty(request.Search) || opePeriodo.Denominacion.Contains(request.Search)) &&
        (!request.Id.HasValue || opePeriodo.Id == request.Id) &&
        (opePeriodo.Borrado != true)
        )
    {
   


    }
}
