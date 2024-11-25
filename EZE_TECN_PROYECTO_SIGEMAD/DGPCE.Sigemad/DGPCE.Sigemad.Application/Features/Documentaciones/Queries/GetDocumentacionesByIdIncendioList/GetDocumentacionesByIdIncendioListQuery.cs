using DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Documentciones.Queries.GetDocumentacionesByIdIncendioList;

public class GetDocumentacionesByIdIncendioListQuery : IRequest<IReadOnlyList<DocumentacionVm>>
{
    public int IdIncendio { get; set; }


    public GetDocumentacionesByIdIncendioListQuery(int id)
    {
        IdIncendio = id;
    }
}