using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Incendios;

public class IncendiosSpecification : BaseSpecification<Incendio>
{
    public IncendiosSpecification(IncendiosSpecificationParams incendioParams)
        : base(incendio =>
        (string.IsNullOrEmpty(incendioParams.Search) || incendio.Denominacion.Contains(incendioParams.Search)) &&
        (!incendioParams.IdTerritorio.HasValue || incendio.IdTerritorio == incendioParams.IdTerritorio) &&
        (!incendioParams.IdCcaa.HasValue || incendio.Provincia.IdCcaa == incendioParams.IdCcaa) &&
        (!incendioParams.IdProvincia.HasValue || incendio.IdProvincia == incendioParams.IdProvincia) &&
        (!incendioParams.IdMunicipio.HasValue || incendio.IdMunicipio == incendioParams.IdMunicipio) &&
        (!incendioParams.IdEstado.HasValue || incendio.IdEstado == incendioParams.IdEstado) &&
        //(!incendioParams.IdEpisodio.HasValue || incendio.IdTerritorio == incendioParams.IdTerritorio) &&
        (!incendioParams.IdNivelGravedad.HasValue || incendio.IdPrevisionPeligroGravedad == incendioParams.IdNivelGravedad) &&
        //(!incendioParams.IdSuperficieAfectada.HasValue || incendio.id == incendioParams.IdTerritorio) &&
        (!incendioParams.FechaInicio.HasValue || incendio.FechaInicio >= incendioParams.FechaInicio)
        //(!incendioParams.FechaFin.HasValue || incendio. >= incendioParams.FechaInicio) && Inicio) &&
        )
    {
        AddInclude(i => i.Territorio);
        AddInclude(i => i.Suceso);
        AddInclude(i => i.Municipio);
        AddInclude(i => i.Provincia);
        AddInclude(i => i.ClaseSuceso);
        AddInclude(i => i.NivelGravedad);
        AddInclude(i => i.EstadoIncendio);

        ApplyPaging(incendioParams);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(incendioParams.Sort?.ToLower()))
        {
            switch (incendioParams.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaInicio);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaInicio);
                    break;
                case "denominacionasc":
                    AddOrderBy(i => i.Denominacion);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Denominacion);
                    break;
                case "nivelgravedadasc":
                    AddOrderBy(i => i.IdPrevisionPeligroGravedad);
                    break;
                case "nivelgravedaddesc":
                    AddOrderByDescending(i => i.IdPrevisionPeligroGravedad);
                    break;
                case "estadodasc":
                    AddOrderBy(i => i.IdEstado);
                    break;
                case "estadodesc":
                    AddOrderByDescending(i => i.IdEstado);
                    break;
                default:
                    AddOrderBy(i => i.FechaInicio); // Orden por defecto
                    break;
            }
        }

    }
}
