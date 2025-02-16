using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.Direcciones;
public class DireccionDto
{
    public int Id { get; set; }
    public TipoDireccionEmergencia TipoDireccionEmergencia { get; set; }

    public string AutoridadQueDirige { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public bool EsEliminable { get; set; }
}
