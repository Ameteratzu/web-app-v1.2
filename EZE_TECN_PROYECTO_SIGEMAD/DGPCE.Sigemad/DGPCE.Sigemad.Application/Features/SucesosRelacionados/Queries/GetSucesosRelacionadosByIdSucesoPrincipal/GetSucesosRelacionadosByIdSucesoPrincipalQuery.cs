using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesosRelacionadosByIdSucesoPrincipal;
public class GetSucesosRelacionadosByIdSucesoPrincipalQuery : IRequest<List<SucesoRelacionadoVm>>
{
    public int IdSucesoPrincipal { get; set; }
    public GetSucesosRelacionadosByIdSucesoPrincipalQuery(int idSucesoPrincipal)
    {
        IdSucesoPrincipal = idSucesoPrincipal;
    }
}
