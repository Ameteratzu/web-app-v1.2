using MediatR;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Delete;

public class DeleteDireccionCoordinacionEmergenciaCommand : IRequest
{
    public int Id { get; set; }
}