using DGPCE.Sigemad.Application.Dtos.ProcedenciasDestinos;
using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
public class DetalleDocumentacionBusquedaDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public TipoDocumento TipoDocumento { get; set; }
    public string Descripcion { get; set; }
    public Guid? IdArchivo { get; set; }

    public List<ProcedenciaDto> ProcedenciaDestinos { get; set; }
}
