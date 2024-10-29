
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaById;
public class GetAreaAfectadaByIdQuery : IRequest<AreaAfectadaVm>
{
    public int Id { get; set; }

    public GetAreaAfectadaByIdQuery(int id)
    {
        Id = id;
    }
}


