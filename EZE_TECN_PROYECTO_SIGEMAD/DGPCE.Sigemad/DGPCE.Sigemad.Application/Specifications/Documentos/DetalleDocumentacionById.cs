using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
public class DetalleDocumentacionById : BaseSpecification<Documentacion>
{
    public DetalleDocumentacionById(int id)
        : base(e => e.Id == id && e.Borrado == false)
    {
        AddInclude(i => i.DetallesDocumentacion); 
        AddInclude("DetallesDocumentacion.DocumentacionProcedenciaDestinos.ProcedenciaDestino");
        AddInclude("DetallesDocumentacion.TipoDocumento");

    }
}
