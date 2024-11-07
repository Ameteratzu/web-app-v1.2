using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Incendios;

public class IncendiosForCountingSpecification : BaseSpecification<Incendio>
{
    public IncendiosForCountingSpecification(IncendiosSpecificationParams request)
        : base(incendio =>
        (string.IsNullOrEmpty(request.Search) || incendio.Denominacion.Contains(request.Search)) &&
        (!request.Id.HasValue || incendio.Id == request.Id) &&
        (!request.IdTerritorio.HasValue || incendio.IdTerritorio == request.IdTerritorio) &&
        (!request.IdPais.HasValue || incendio.IdPais == request.IdPais) &&
        (!request.IdCcaa.HasValue || incendio.Provincia.IdCcaa == request.IdCcaa) &&
        (!request.IdProvincia.HasValue || incendio.IdProvincia == request.IdProvincia) &&
        (!request.IdMunicipio.HasValue || incendio.IdMunicipio == request.IdMunicipio) &&
        (!request.IdEstadoSuceso.HasValue || incendio.IdEstadoSuceso == request.IdEstadoSuceso) &&
        (incendio.Borrado != true)
        )
    {
        if (request.IdEstadoIncendio.HasValue)
        {
            AddInclude(i => i.Evoluciones);
            AddCriteria(i => i.Evoluciones.Any(e => e.IdEstadoIncendio == request.IdEstadoIncendio.Value));
        }

        if (request.IdMovimiento == MovimientoTipos.Registro && request.IdComparativoFecha.HasValue)
        {
            switch (request.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) == request.FechaInicio);
                    break;
                case ComparacionTipos.MayorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) > request.FechaInicio);
                    break;
                case ComparacionTipos.MenorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) < request.FechaInicio);
                    break;
                case ComparacionTipos.Entre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) >= request.FechaInicio && DateOnly.FromDateTime(incendio.FechaCreacion) <= request.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) < request.FechaInicio || DateOnly.FromDateTime(incendio.FechaCreacion) > request.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'No Entre'");
                    }
                    break;
                default:
                    throw new ArgumentException("Operador de comparar fechas no válido");
            }
        }
        else if (request.IdMovimiento == MovimientoTipos.InicioSuceso && request.IdComparativoFecha.HasValue)
        {
            switch (request.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio) == request.FechaInicio);
                    break;
                case ComparacionTipos.MayorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio) > request.FechaInicio);
                    break;
                case ComparacionTipos.MenorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio) < request.FechaInicio);
                    break;
                case ComparacionTipos.Entre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio) >= request.FechaInicio && DateOnly.FromDateTime(incendio.FechaInicio) <= request.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaInicio) < request.FechaInicio || DateOnly.FromDateTime(incendio.FechaInicio) > request.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'No Entre'");
                    }
                    break;
                default:
                    throw new ArgumentException("Operador de comparar fechas no válido");
            }
        }
        else if (request.IdMovimiento == MovimientoTipos.Modificacion && request.IdComparativoFecha.HasValue)
        {
            switch (request.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) == request.FechaInicio);
                    break;
                case ComparacionTipos.MayorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) > request.FechaInicio);
                    break;
                case ComparacionTipos.MenorQue:
                    AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) < request.FechaInicio);
                    break;
                case ComparacionTipos.Entre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) >= request.FechaInicio && DateOnly.FromDateTime(incendio.FechaModificacion.Value) <= request.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaModificacion.Value) < request.FechaInicio || DateOnly.FromDateTime(incendio.FechaModificacion.Value) > request.FechaFin);
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'No Entre'");
                    }
                    break;
                default:
                    throw new ArgumentException("Operador de comparar fechas no válido");
            }
        }
    }
}
