using DGPCE.Sigemad.Application.Dtos.Impactos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
public class CreateListaImpactosCommand: IRequest<CreateListaImpactosResponse>
{
    public int? IdEvolucion { get; set; }
    public int IdIncendio { get; set; } // Se usará si no se recibe el IdEvolucion
    public List<CreateImpactoEvolucionDto> Impactos { get; set; } = new();
}
