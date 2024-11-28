using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucionById;

public class GetEvolucionByIdQuery : IRequest<EvolucionDto>
{
    public int Id { get; set; }

    public GetEvolucionByIdQuery(int id)
    {
        Id = id;
    }
}
