using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.DeleteImpactoEvoluciones;
public class DeleteImpactoEvolucionCommand : IRequest
{
    public int Id { get; set; }
}
