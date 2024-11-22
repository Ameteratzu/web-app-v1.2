
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaById;
public class GetAreaAfectadaByIdQuery : IRequest<AreaAfectadaDto>
{
    public int Id { get; set; }

    public GetAreaAfectadaByIdQuery(int id)
    {
        Id = id;
    }
}


