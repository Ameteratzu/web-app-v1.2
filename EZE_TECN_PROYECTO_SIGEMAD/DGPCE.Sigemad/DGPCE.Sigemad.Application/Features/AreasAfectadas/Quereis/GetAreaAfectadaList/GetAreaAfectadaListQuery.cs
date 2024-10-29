using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
public class GetAreaAfectadaListQuery : IRequest<IReadOnlyList<AreaAfectadaVm>>
{
    public int IdEvolucion { get; set; }

    public GetAreaAfectadaListQuery(int id)
    {
        IdEvolucion = id;
    }
}
