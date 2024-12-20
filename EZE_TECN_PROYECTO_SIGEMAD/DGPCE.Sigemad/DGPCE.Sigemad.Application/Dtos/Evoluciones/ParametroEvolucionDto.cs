using DGPCE.Sigemad.Domain.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.Evoluciones;
public class ParametroEvolucionDto
{

    public DateTime? FechaFinal { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public string? PlanEmergenciaActivado { get; set; }

    public EstadoIncendio EstadoIncendio { get; set; }

    public FaseEmergencia? Fase { get; set; }

    public SituacionOperativa? SituacionOperativa { get; set; }

    public SituacionEquivalente? SituacionEquivalente { get; set; }
}
