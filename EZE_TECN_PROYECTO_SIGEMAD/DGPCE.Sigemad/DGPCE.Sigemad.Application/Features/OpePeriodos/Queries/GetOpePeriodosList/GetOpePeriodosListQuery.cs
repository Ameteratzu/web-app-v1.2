
using DGPCE.Sigemad.Application.Features.Periodos.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Periodos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Periodos.Queries.GetPeriodosList
{

    public class GetOpePeriodosListQuery : OpePeriodosSpecificationParams, IRequest<PaginationVm<OpePeriodoVm>>
    {
    }
}
