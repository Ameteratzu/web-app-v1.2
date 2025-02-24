
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Periodos.Queries.GetPeriodoById
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
