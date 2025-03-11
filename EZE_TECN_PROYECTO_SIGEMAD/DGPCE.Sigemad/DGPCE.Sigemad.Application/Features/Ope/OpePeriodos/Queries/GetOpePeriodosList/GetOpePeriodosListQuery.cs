using DGPCE.Sigemad.Application.Features.Ope.OpePeriodos.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.OpePeriodos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.OpePeriodos.Queries.GetOpePeriodosList
{

    public class GetOpePeriodosListQuery : OpePeriodosSpecificationParams, IRequest<PaginationVm<OpePeriodoVm>>
    {
    }
}
