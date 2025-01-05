using DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDocumentacionesByIdIncendioList;

public class GetDocumentacionesByIdSucesoListQuery : IRequest<IReadOnlyList<DocumentacionVm>>
{
    public int IdSuceso { get; set; }


    public GetDocumentacionesByIdSucesoListQuery(int id)
    {
        IdSuceso = id;
    }
}