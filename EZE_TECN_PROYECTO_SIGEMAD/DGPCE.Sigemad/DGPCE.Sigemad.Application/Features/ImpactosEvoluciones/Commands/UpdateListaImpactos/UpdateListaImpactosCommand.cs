using DGPCE.Sigemad.Application.Dtos.Impactos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateListaImpactos;
public class UpdateListaImpactosCommand: IRequest<UpdateListaImpactosResponse>
{
    public int IdEvolucion { get; set; }
    public List<UpdateImpactoEvolucionDto> Impactos { get; set; } = new();
}
