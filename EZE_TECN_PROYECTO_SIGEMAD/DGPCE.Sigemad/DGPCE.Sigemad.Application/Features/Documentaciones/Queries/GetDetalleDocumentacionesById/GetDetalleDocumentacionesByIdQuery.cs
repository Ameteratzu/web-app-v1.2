using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDetalleDocumentacionesById;
public class GetDetalleDocumentacionesByIdQuery : IRequest<DocumentacionDto>
{
    public int Id { get; set; }

    public GetDetalleDocumentacionesByIdQuery(int id)
    {
        Id = id;
    }
}