
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;
public class ManageDocumentacionesCommand
{
    public int? Id { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public int IdTipoDocumento { get; set; }
    public string Descripcion { get; set; }
    public Guid IdArchivo { get; set; }
    public ICollection<int>? DocumentacionProcedenciasDestinos { get; set; }
}
