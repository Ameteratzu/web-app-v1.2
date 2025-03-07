using DGPCE.Sigemad.Application.Dtos.DireccionCoordinaciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionEmergencia;
public class GetDireccionCoordinacionEmergenciaQuery : IRequest<DireccionCoordinacionEmergenciaDto>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
}
