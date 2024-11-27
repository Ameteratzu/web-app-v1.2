using DGPCE.Sigemad.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Documentacion : BaseDomainModel<int>
{
    public int IdIncendio { get; set; }

    public Incendio Incendio { get; set; }
    public List<DetalleDocumentacion> DetalleDocumentaciones { get; set; } = null;
}