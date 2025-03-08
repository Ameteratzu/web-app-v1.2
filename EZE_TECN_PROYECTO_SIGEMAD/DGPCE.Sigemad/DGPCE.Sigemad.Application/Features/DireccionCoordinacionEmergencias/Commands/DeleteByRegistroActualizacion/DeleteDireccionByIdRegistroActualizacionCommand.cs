using MediatR;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.DeleteByRegistroActualizacion;
public class DeleteDireccionByIdRegistroActualizacionCommand : IRequest
{
    public int IdRegistroActualizacion { get; set; }
}
