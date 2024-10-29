using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
public class GetAreasAfectadasByIdEvolucionQuery : IRequest<IReadOnlyList<AreaAfectadaVm>>
{
    public int IdEvolucion { get; set; }

    public GetAreasAfectadasByIdEvolucionQuery(int id)
    {
        IdEvolucion = id;
    }
}
