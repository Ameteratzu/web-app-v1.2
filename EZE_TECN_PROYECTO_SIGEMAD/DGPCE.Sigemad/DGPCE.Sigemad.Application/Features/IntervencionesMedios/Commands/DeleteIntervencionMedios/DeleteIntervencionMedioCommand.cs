using MediatR;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.DeleteIntervencionMedios;
public class DeleteIntervencionMedioCommand : IRequest
{
    public int Id { get; set; }
}
