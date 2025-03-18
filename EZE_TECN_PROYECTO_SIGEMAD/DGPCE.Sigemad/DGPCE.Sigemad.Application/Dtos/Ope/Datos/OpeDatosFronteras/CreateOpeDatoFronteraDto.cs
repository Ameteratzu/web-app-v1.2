namespace DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
public class CreateOpeDatoFronteraDto : IEquatable<CreateOpeDatoFronteraDto>
{
    public int? Id { get; set; }

    public int idOpeFrontera { get; set; }
    public DateTime FechaHoraInicioIntervalo { get; set; }
    public DateTime FechaHoraFinIntervalo { get; set; }
    public string NumeroVehiculos { get; set; }
    public string Afluencia { get; set; }
  

    public bool Equals(CreateOpeDatoFronteraDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            idOpeFrontera == other.idOpeFrontera &&
            NumeroVehiculos == other.NumeroVehiculos &&
            string.Equals(Afluencia, other.Afluencia) &&
            FechaHoraInicioIntervalo == other.FechaHoraInicioIntervalo &&
            FechaHoraFinIntervalo == other.FechaHoraFinIntervalo;
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateOpeDatoFronteraDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            idOpeFrontera,
            FechaHoraInicioIntervalo,
            FechaHoraFinIntervalo,
            NumeroVehiculos,
            Afluencia);
    }
}
