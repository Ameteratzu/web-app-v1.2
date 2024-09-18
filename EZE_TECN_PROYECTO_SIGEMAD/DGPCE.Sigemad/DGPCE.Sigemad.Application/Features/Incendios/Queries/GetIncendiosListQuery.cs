using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries
{
    public class GetIncendiosListQuery: PaginationBaseQuery, IRequest<PaginationVm<Incendio>>
    {
    }
}
