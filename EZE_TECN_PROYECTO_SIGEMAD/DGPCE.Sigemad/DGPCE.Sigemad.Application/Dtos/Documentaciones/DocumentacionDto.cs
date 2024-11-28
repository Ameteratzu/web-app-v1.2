using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.Documentaciones;
public class DocumentacionDto
{
    public int Id { get; set; }
    public int IdIncendio { get; set; }

    public List<DetalleDocumentacionBusquedaDto>? DetallesDocumentacion { get; set; }
}
