using DGPCE.Sigemad.Application.Dtos.Registros;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetRegistrosDeIncendio;
public class GetRegistrosPorIncendioQuery : IRequest<IReadOnlyList<RegistroActualizacionDto>>
{
    public int IdIncendio { get; set; }

    public GetRegistrosPorIncendioQuery(int idIncendio)
    {
        IdIncendio = idIncendio;
    }
}
