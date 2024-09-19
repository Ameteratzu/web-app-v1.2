namespace DGPCE.Sigemad.Application.Specifications.Incendios;

public class IncendiosSpecificationParams: SpecificationParams
{
    public int? IdTerritorio { get; set; }
    public int? IdCcaa { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }
    public int? IdEstado { get; set; }
    public int? IdEpisodio { get; set; }
    public int? IdNivelGravedad { get; set; }
    public int? IdSuperficieAfectada { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}