using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Documentos;
public class DocumentoSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public int? IdIncendio { get; set; }
    public int? IdTipoDocumento { get; set; }
    public string? Descripcion { get; set; }
    public Guid? IdArchivo { get; set; }
}
