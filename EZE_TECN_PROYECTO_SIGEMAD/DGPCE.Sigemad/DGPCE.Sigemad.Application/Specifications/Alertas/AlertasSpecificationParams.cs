using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Specifications.Alertas
{
    public class AlertasSpecificationParams : SpecificationParams
    {
        public Guid? IdEstado { get; set; }
        public DateTime? FechaAlerta { get; set; }
    }
}
