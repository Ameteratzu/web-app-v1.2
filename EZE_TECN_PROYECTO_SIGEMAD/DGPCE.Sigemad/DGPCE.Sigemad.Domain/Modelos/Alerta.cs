using DGPCE.Sigemad.Domain.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class Alerta : BaseDomainModel
    {
        public Alerta()
        {
        }

        public string? Descripcion { get; set; }

        public DateTime? FechaAlerta { get; set; }

        public Guid EstadoAlertaId { get; set; }
        public EstadoAlerta? EstadoAlerta { get; set; }

    }

}
