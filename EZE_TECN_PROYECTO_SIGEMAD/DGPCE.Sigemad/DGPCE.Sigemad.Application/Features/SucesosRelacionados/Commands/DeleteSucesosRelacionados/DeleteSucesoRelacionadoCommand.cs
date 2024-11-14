using MediatR;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesosRelacionados;
public class DeleteSucesoRelacionadoCommand : IRequest
{
    public int IdSucesoPrincipal { get; set; }
    public int IdSucesoAsociado { get; set; }
}
