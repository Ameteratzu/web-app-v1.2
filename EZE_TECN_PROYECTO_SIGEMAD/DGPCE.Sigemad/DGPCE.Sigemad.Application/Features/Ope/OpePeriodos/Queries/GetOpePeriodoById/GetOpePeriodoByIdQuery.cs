using DGPCE.Sigemad.Domain.Modelos.Ope;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Ope.OpePeriodos.Queries.GetOpePeriodoById
{
    public class GetOpePeriodoByIdQuery : IRequest<OpePeriodo>
    {
        public int Id { get; set; }

        public GetOpePeriodoByIdQuery(int id)
        {
            Id = id;
        }
    }
}
