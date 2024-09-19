using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Incendios;

public class IncendiosForCountingSpecification : BaseSpecification<Incendio>
{
    public IncendiosForCountingSpecification(IncendiosSpecificationParams incendioParams)
        : base(incendio =>
        (string.IsNullOrEmpty(incendioParams.Search) || incendio.Denominacion.Contains(incendioParams.Search)) &&
        (!incendioParams.IdTerritorio.HasValue || incendio.IdTerritorio == incendioParams.IdTerritorio) &&
        (!incendioParams.IdCcaa.HasValue || incendio.Provincia.IdCcaa == incendioParams.IdCcaa) &&
        (!incendioParams.IdProvincia.HasValue || incendio.IdProvincia == incendioParams.IdProvincia) &&
        (!incendioParams.IdMunicipio.HasValue || incendio.IdMunicipio == incendioParams.IdMunicipio) &&
        //(!incendioParams.IdEstado.HasValue || incendio. == incendioParams.IdEstado) &&
        //(!incendioParams.IdEpisodio.HasValue || incendio.IdTerritorio == incendioParams.IdTerritorio) &&
        (!incendioParams.IdNivelGravedad.HasValue || incendio.IdPrevisionPeligroGravedad == incendioParams.IdNivelGravedad) &&
        //(!incendioParams.IdSuperficieAfectada.HasValue || incendio.id == incendioParams.IdTerritorio) &&
        (!incendioParams.FechaInicio.HasValue || incendio.FechaInicio >= incendioParams.FechaInicio)
        //(!incendioParams.FechaFin.HasValue || incendio. >= incendioParams.FechaInicio) && Inicio) &&
        )
    {
    }
}
