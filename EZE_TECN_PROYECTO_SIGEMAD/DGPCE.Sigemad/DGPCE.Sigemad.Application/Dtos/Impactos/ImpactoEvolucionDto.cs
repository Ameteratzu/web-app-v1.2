using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Dtos.Impactos;
public class ImpactoEvolucionDto
{
    public bool? Nuclear { get; set; }
    public int? ValorAD { get; set; }
    public int? Numero { get; set; }
    public string? Observaciones { get; set; }
    public DateTime? Fecha { get; set; }
    public DateTime? FechaHora { get; set; }
    public DateTime? FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public char? AlteracionInterrupcion { get; set; }
    public string? Causa { get; set; }
    public int? NumeroGraves { get; set; }
    public Geometry? ZonaPlanificacion { get; set; }

    public int? NumeroUsuarios { get; set; }
    public int? NumeroIntervinientes { get; set; }
    public int? NumeroServicios { get; set; }
    public int? NumeroLocalidades { get; set; }

    public ImpactoClasificado? ImpactoClasificado { get; set; }

    public TipoDanio? TipoDanio { get; set; }
}
