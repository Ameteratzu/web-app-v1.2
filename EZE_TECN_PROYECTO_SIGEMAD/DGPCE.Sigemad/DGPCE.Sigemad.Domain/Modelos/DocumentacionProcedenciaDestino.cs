using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos;
public class DocumentacionProcedenciaDestino
{
    public int Id { get; set; }
    public int IdDetalleDocumentacion { get; set; }
    public int IdProcedenciaDestino { get; set; }

    public DetalleDocumentacion DetalleDocumentacion { get; set; }
    public ProcedenciaDestino ProcedenciaDestino { get; set; }
}