namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeDatosFronteras;

public class OpeDatosFronterasSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public int? IdOpeFrontera { get; set; }
    public int? IdComparativoFecha { get; set; }
    public DateOnly? FechaHoraInicioIntervalo { get; set; }
    public DateOnly? FechaHoraFinIntervalo { get; set; }
    public int? NumeroVehiculos { get; set; }
    public string? Afluencia { get; set; }

}