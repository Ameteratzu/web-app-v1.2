
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
internal class DocumentoSpecification : BaseSpecification<Documentacion>
{
        public DocumentoSpecification(DocumentoSpecificationParams request)
        : base(documento =>
        (string.IsNullOrEmpty(request.Search) || documento.Descripcion.Contains(request.Search)) &&
        (!request.Id.HasValue || documento.Id == request.Id) &&
        (!request.IdIncendio.HasValue || documento.IdIncendio == request.IdIncendio) &&
        (!request.IdTipoDocumento.HasValue || documento.IdTipoDocumento == request.IdTipoDocumento) &&
        (!request.IdArchivo.HasValue || documento.IdArchivo == request.IdArchivo) &&
        (documento.Borrado != true)
        )
        {

        AddInclude(d => d.DocumentacionProcedenciaDestinos);
       }
}
