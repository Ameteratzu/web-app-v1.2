using DGPCE.Sigemad.Application.Dtos.SucesoRelacionados;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
public class ManageSucesoRelacionadosCommand: IRequest<ManageSucesoRelacionadoResponse>
{
    public int? IdSucesoRelacionado { get; set; }
    public int IdSuceso { get; set; }
    public List<int> IdsSucesosAsociados { get; set; }
}
