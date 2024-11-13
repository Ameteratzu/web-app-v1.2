using MediatR;
using System.Text.Json.Serialization;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.CreateSucesosRelacionados;
public class CreateSucesoRelacionadoCommand : IRequest<CreateSucesoRelacionadoResponse>
{
    [JsonIgnore]
    public int IdSucesoPrincipal { get; set; }
    public int IdSucesoAsociado { get; set; }
    public string Observaciones { get; set; }
}
