using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Parametros.Commands;
public class CreateParametroCommand
{
    public int IdEstadoIncendio { get; set; }
    public DateTime? FechaFinal { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public string? PlanEmergenciaActivado { get; set; }

    public int? IdFase { get; set; }

    public int? IdSituacionOperativa { get; set; }

    public int? IdSituacionEquivalente { get; set; }
}
