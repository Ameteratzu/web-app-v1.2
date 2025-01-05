using DGPCE.Sigemad.Application.Dtos.Registros;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorIncendio;
public class GetRegistrosPorSucesoQuery : IRequest<IReadOnlyList<RegistroActualizacionDto>>
{
    public int IdSuceso { get; set; }

    public GetRegistrosPorSucesoQuery(int idSuceso)
    {
        IdSuceso = idSuceso;
    }
}
