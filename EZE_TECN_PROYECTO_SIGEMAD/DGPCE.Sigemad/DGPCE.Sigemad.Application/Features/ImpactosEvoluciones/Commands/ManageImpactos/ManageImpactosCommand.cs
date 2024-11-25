using DGPCE.Sigemad.Application.Dtos.Impactos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
public class ManageImpactosCommand: IRequest<ManageImpactoResponse>
{
    public int? IdEvolucion { get; set; }
    public int IdIncendio { get; set; } // Se usará si no se recibe el IdEvolucion
    public List<ManageImpactoDto> Impactos { get; set; } = new();
}
