using DGPCE.Sigemad.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos;
public class DetalleDocumentacion : BaseDomainModel<int>
{
    public int? IdDocumentacion { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public int IdTipoDocumento { get; set; }
    public string Descripcion { get; set; }
    public Guid? IdArchivo { get; set; }
    public TipoDocumento TipoDocumento { get; set; }
    public Archivo? Archivo { get; set; }
    public Documentacion Documentacion { get; set; }

    public ICollection<DocumentacionProcedenciaDestino>? DocumentacionProcedenciaDestinos { get; set; } = null;

}
