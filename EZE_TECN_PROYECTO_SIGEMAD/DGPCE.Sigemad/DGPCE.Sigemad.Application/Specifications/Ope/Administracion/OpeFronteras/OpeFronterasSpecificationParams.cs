namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFronteras;

public class OpeFronterasSpecificationParams : SpecificationParams
{
    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;
    public int? IdCcaa { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }
    public string? CarreteraPK { get; set; }
    public string? CoordenadaUTM_X { get; set; }
    public string? CoordenadaUTM_Y { get; set; }
    public int? TransitoMedioVehiculos { get; set; }
    public int? TransitoAltoVehiculos { get; set; }

}