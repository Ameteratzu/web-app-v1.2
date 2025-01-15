using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
public class DetalleDocumentacionById : BaseSpecification<Documentacion>
{
    public DetalleDocumentacionById(int id)
        : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(i => i.DetallesDocumentacion.Where(d => !d.Borrado));
        AddInclude("DetallesDocumentacion.DocumentacionProcedenciaDestinos.ProcedenciaDestino");
        AddInclude("DetallesDocumentacion.TipoDocumento");
        AddInclude("DetallesDocumentacion.Archivo");

    }
}
