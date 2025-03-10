using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.ManageIntervencionMedios;
public class ManageIntervencionMedioCommand : IRequest<ManageIntervencionMedioResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }

    public List<ManageIntervencionMedioDto> Intervenciones { get; set; } = new();
}
