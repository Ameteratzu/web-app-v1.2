using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
public class GetAreasAfectadasByIdEvolucionQuery : IRequest<IReadOnlyList<AreaAfectadaDto>>
{
    public int IdEvolucion { get; set; }

    public GetAreasAfectadasByIdEvolucionQuery(int id)
    {
        IdEvolucion = id;
    }
}
