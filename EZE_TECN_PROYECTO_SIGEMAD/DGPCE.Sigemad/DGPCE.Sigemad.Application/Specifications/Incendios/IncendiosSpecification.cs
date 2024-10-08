using DGPCE.Sigemad.Domain.Modelos;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Specifications.Incendios;

public class IncendiosSpecification : BaseSpecification<Incendio>
{
    public IncendiosSpecification(IncendiosSpecificationParams incendioParams)
        : base(incendio =>
        (string.IsNullOrEmpty(incendioParams.Search) || incendio.Denominacion.Contains(incendioParams.Search)) &&
        (!incendioParams.Id.HasValue || incendio.Id == incendioParams.Id) &&
        (!incendioParams.IdTerritorio.HasValue || incendio.IdTerritorio == incendioParams.IdTerritorio) &&
        (!incendioParams.IdCcaa.HasValue || incendio.Provincia.IdCcaa == incendioParams.IdCcaa) &&
        (!incendioParams.IdProvincia.HasValue || incendio.IdProvincia == incendioParams.IdProvincia) &&
        (!incendioParams.IdMunicipio.HasValue || incendio.IdMunicipio == incendioParams.IdMunicipio) &&
        //(!incendioParams.IdEstado.HasValue || incendio.IdEstado == incendioParams.IdEstado) &&
        //(!incendioParams.IdEpisodio.HasValue || incendio.IdTerritorio == incendioParams.IdTerritorio) &&
        (!incendioParams.IdNivelGravedad.HasValue || incendio.IdPrevisionPeligroGravedad == incendioParams.IdNivelGravedad) &&
        //(!incendioParams.IdSuperficieAfectada.HasValue || incendio.id == incendioParams.IdTerritorio) &&
        (!incendioParams.FechaInicio.HasValue || incendio.FechaInicio >= incendioParams.FechaInicio) &&
        (incendio. Borrado != true)
        //(!incendioParams.FechaFin.HasValue || incendio. >= incendioParams.FechaInicio) && Inicio) &&
        )
    {

        /*
        // Filtro dinámico de fechas
        Func<Incendio, DateTime?> fechaFiltro = null;

        switch (incendioParams.IdMovimiento)
        {
            case 1: // Registro
                fechaFiltro = i => i.FechaCreacion;
                break;
            case 2: // Inicio Suceso
                fechaFiltro = i => i.FechaInicio;
                break;
            case 3: // Modificación
                fechaFiltro = i => i.FechaModificacion;
                break;
            case 4: // Cualquiera (aplicar a todas las fechas)
                AddCriteria(i =>
                    (i.FechaCreacion >= incendioParams.FechaInicio && i.FechaCreacion <= incendioParams.FechaFin) ||
                    (i.FechaInicio >= incendioParams.FechaInicio && i.FechaInicio <= incendioParams.FechaFin) ||
                    (i.FechaModificacion >= incendioParams.FechaInicio && i.FechaModificacion <= incendioParams.FechaFin)
                );
                return; // No continuamos si es "Cualquiera"
        }

        if (fechaFiltro != null)
        {
            // Aplicar comparativos
            switch (incendioParams.IdComparativoFecha)
            {
                case 1: // Entre
                    if (incendioParams.FechaInicio.HasValue && incendioParams.FechaFin.HasValue)
                    {
                        AddCriteria(i =>
                            fechaFiltro(i) >= incendioParams.FechaInicio &&
                            fechaFiltro(i) <= incendioParams.FechaFin
                        );
                    }
                    break;

                case 2: // Igual a
                    if (incendioParams.FechaInicio.HasValue)
                    {
                        AddCriteria(i =>
                            fechaFiltro(i) == incendioParams.FechaInicio
                        );
                    }
                    break;

                case 3: // Mayor que
                    if (incendioParams.FechaInicio.HasValue)
                    {
                        AddCriteria(i =>
                            fechaFiltro(i) > incendioParams.FechaInicio
                        );
                    }
                    break;

                case 4: // Menor que
                    if (incendioParams.FechaInicio.HasValue)
                    {
                        AddCriteria(i =>
                            fechaFiltro(i) < incendioParams.FechaInicio
                        );
                    }
                    break;

                case 5: // No entre
                    if (incendioParams.FechaInicio.HasValue && incendioParams.FechaFin.HasValue)
                    {
                        AddCriteria(i =>
                            fechaFiltro(i) < incendioParams.FechaInicio ||
                            fechaFiltro(i) > incendioParams.FechaFin
                        );
                    }
                    break;
            }
        }
        */

        // Filtro dinámico de fechas
        if (incendioParams.IdMovimiento.HasValue && incendioParams.IdComparativoFecha.HasValue)
        {
            Func<Incendio, DateTime?> fechaFiltro = null;

            switch (incendioParams.IdMovimiento)
            {
                case 1: // Registro
                    fechaFiltro = i => i.FechaCreacion;
                    break;
                case 2: // Inicio Suceso
                    fechaFiltro = i => i.FechaInicio;
                    break;
                case 3: // Modificación
                    fechaFiltro = i => i.FechaModificacion;
                    break;
            }

            if (fechaFiltro != null)
            {
                // Aplicar comparativos
                switch (incendioParams.IdComparativoFecha)
                {
                    case 1: // Entre
                        if (incendioParams.FechaInicio.HasValue && incendioParams.FechaFin.HasValue)
                        {
                            AddCriteria(i =>
                                fechaFiltro(i) >= incendioParams.FechaInicio &&
                                fechaFiltro(i) <= incendioParams.FechaFin
                            );
                        }
                        break;

                    case 2: // Igual a
                        if (incendioParams.FechaInicio.HasValue)
                        {
                            AddCriteria(i =>
                                fechaFiltro(i) == incendioParams.FechaInicio
                            );
                        }
                        break;

                    case 3: // Mayor que
                        if (incendioParams.FechaInicio.HasValue)
                        {
                            AddCriteria(i =>
                                fechaFiltro(i) > incendioParams.FechaInicio
                            );
                        }
                        break;

                    case 4: // Menor que
                        if (incendioParams.FechaInicio.HasValue)
                        {
                            AddCriteria(i =>
                                fechaFiltro(i) < incendioParams.FechaInicio
                            );
                        }
                        break;

                    case 5: // No entre
                        if (incendioParams.FechaInicio.HasValue && incendioParams.FechaFin.HasValue)
                        {
                            AddCriteria(i =>
                                fechaFiltro(i) < incendioParams.FechaInicio ||
                                fechaFiltro(i) > incendioParams.FechaFin
                            );
                        }
                        break;
                }
            }
        }


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
