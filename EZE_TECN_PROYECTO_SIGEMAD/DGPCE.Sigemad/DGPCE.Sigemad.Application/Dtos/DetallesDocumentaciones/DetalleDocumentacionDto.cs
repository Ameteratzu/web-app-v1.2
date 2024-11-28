using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
public class DetalleDocumentacionDto
{
    public int? Id { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public int IdTipoDocumento { get; set; }
    public string Descripcion { get; set; }
    public Guid? IdArchivo { get; set; }

    public List<int>? DocumentacionProcedenciasDestinos { get; set; }= new ();

}