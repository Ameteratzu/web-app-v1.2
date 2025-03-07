namespace DGPCE.Sigemad.Application.Dtos.Direcciones;
public class CreateOrUpdateDireccionDto: IEquatable<CreateOrUpdateDireccionDto>
{
    public int? Id { get; set; }
    public int IdTipoDireccionEmergencia { get; set; }
    public string AutoridadQueDirige { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }

    public bool Equals(CreateOrUpdateDireccionDto? other)
    {
        if(other is null)
        {
            return false;
        }

        return Id == other.Id && 
            IdTipoDireccionEmergencia == other.IdTipoDireccionEmergencia &&
            string.Equals(AutoridadQueDirige, other.AutoridadQueDirige) && 
            FechaInicio == other.FechaInicio && 
            FechaFin == other.FechaFin;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CreateOrUpdateDireccionDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, 
            IdTipoDireccionEmergencia, 
            AutoridadQueDirige ?? string.Empty, 
            FechaInicio, 
            FechaFin ?? default);
    }
}
