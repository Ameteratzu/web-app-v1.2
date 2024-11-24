using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.DatosPrincipales.Commands;
public class CreateDatoPrincipalCommand
{
    public DateTime FechaHora { get; set; }
    public string? Observaciones { get; set; }
    public string? Prevision { get; set; }
}
