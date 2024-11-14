using MediatR;
using System.Text.Json.Serialization;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.UpdateSucesosRelacionados;
public class UpdateSucesoRelacionadoCommand :IRequest
{
    public int Id { get; set; }
    [JsonIgnore]
    public int IdSucesoPrincipal { get; set; }
    [JsonIgnore]
    public int IdSucesoAsociado { get; set; }
    public string Observaciones { get; set; }
}
