using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Common
{
    public abstract class BaseDomainModel
    {
        public Guid Id { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? ModificadoPor { get; set; }
    }
}
