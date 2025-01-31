using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
internal class DocumentacionSpecificationByIdSuceso : BaseSpecification<Documentacion>
{
    public DocumentacionSpecificationByIdSuceso(int idSuceso)
    : base(d => d.IdSuceso == idSuceso && d.Borrado == false)
    {
        AddInclude(d => d.DetallesDocumentacion);
    }
}
