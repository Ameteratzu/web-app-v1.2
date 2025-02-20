using System.Text.Json.Serialization;

namespace DGPCE.Sigemad.Application.Specifications.Periodos;

public class OpePeriodosSpecificationParams: SpecificationParams
{
    public int? Id { get; set; }

    public int? IdComparativoFecha { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }

}