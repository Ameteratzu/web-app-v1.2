using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Incendios;

public class IncendiosSpecification : BaseSpecification<Incendio>
{
    public IncendiosSpecification(IncendiosSpecificationParams request)
        : base(incendio =>
        (string.IsNullOrEmpty(request.Search) || incendio.Denominacion.Contains(request.Search)) &&
        (!request.Id.HasValue || incendio.Id == request.Id) &&
        (!request.IdTerritorio.HasValue || incendio.IdTerritorio == request.IdTerritorio) &&
        (!request.IdPais.HasValue || incendio.IdPais == request.IdPais) &&
        (!request.IdCcaa.HasValue || incendio.Provincia.IdCcaa == request.IdCcaa) &&
        (!request.IdProvincia.HasValue || incendio.IdProvincia == request.IdProvincia) &&
        (!request.IdMunicipio.HasValue || incendio.IdMunicipio == request.IdMunicipio) &&
        (!request.IdClaseSuceso.HasValue || incendio.IdClaseSuceso == request.IdClaseSuceso) &&
        (!request.IdEstadoSuceso.HasValue || incendio.IdEstadoSuceso == request.IdEstadoSuceso) &&
        (!request.IdSuceso.HasValue || incendio.IdSuceso == request.IdSuceso) &&
        (incendio.Borrado != true)
        )
    {

        if (request.IdEstadoIncendio.HasValue)
        {
            AddInclude(i => i.Suceso.Evoluciones);

            AddCriteria(i => i.Suceso.Evoluciones
                .Where(e => e.EsFoto == false && e.Borrado == false)
                .SelectMany(e => e.Parametros)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(1) // Solo tomar 1 registro
                .Any(p => p.IdEstadoIncendio == request.IdEstadoIncendio.Value));
        }

        if (request.IdSituacionEquivalente.HasValue)
        {
            AddInclude(i => i.Suceso.Evoluciones);

            AddCriteria(i => i.Suceso.Evoluciones
                .Where(e => e.EsFoto == false && e.Borrado == false)
                .SelectMany(e => e.Parametros)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(1) // Solo tomar 1 registro
                .Any(p => p.IdSituacionEquivalente == request.IdSituacionEquivalente.Value));
        }

        AddInclude("Suceso.Evoluciones.Parametros.SituacionEquivalente");
        
        AddInclude(i => i.Suceso.RegistroActualizaciones);
        AddOrderByDescending(i => i.Suceso.RegistroActualizaciones.OrderByDescending(ra => ra.FechaCreacion).FirstOrDefault().FechaCreacion);

        if (request.busquedaSucesos != null && (bool)request.busquedaSucesos)
        {
            AddInclude(i => i.Suceso.TipoSuceso);
        }
        else
        {
            AddInclude(i => i.Territorio);
            AddInclude(i => i.Municipio);
            AddInclude(i => i.Provincia);
            AddInclude(i => i.ClaseSuceso);
            AddInclude(i => i.Suceso);
        }

        AddInclude(i => i.EstadoSuceso);

        if (request.IdMovimiento == MovimientoTipos.Registro && request.IdComparativoFecha.HasValue)
        {
            switch (request.IdComparativoFecha.Value)
            {
                case ComparacionTipos.IgualA:

                    //AddCriteria(i => i.Suceso.Evoluciones
                    //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //.Select(e => e.Registro.FechaHoraEvolucion)
                    //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                    //.FirstOrDefault() != null &&
                    //DateOnly.FromDateTime(i.Suceso.Evoluciones
                    //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //    .Select(e => e.Registro.FechaHoraEvolucion)
                    //    .OrderByDescending(fecha => fecha)
                    //    .FirstOrDefault().Value) == request.FechaInicio);

                    AddCriteria(i => i.Suceso.Evoluciones
                        .Where(e => e.EsFoto == false && e.Borrado == false) // Filtrar evoluciones válidas
                        .SelectMany(e => e.Registros) // Seleccionar todos los registros de esas evoluciones
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r => DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) == request.FechaInicio));

                    break;

                case ComparacionTipos.MayorQue:

                    //AddCriteria(i => i.Suceso.Evoluciones
                    //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //.Select(e => e.Registro.FechaHoraEvolucion)
                    //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                    //.FirstOrDefault() != null &&
                    //DateOnly.FromDateTime(i.Suceso.Evoluciones
                    //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //    .Select(e => e.Registro.FechaHoraEvolucion)
                    //    .OrderByDescending(fecha => fecha)
                    //    .FirstOrDefault().Value) > request.FechaInicio);

                    AddCriteria(i => i.Suceso.Evoluciones
                        .Where(e => e.EsFoto == false && e.Borrado == false) // Filtrar evoluciones válidas
                        .SelectMany(e => e.Registros) // Seleccionar todos los registros de esas evoluciones
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r => DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) > request.FechaInicio));

                    break;

                case ComparacionTipos.MenorQue:
                    //AddCriteria(i => i.Suceso.Evoluciones
                    //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //.Select(e => e.Registro.FechaHoraEvolucion)
                    //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                    //.FirstOrDefault() != null &&
                    //DateOnly.FromDateTime(i.Suceso.Evoluciones
                    //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                    //    .Select(e => e.Registro.FechaHoraEvolucion)
                    //    .OrderByDescending(fecha => fecha)
                    //    .FirstOrDefault().Value) < request.FechaInicio);

                    //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) < request.FechaInicio);

                    AddCriteria(i => i.Suceso.Evoluciones
                        .Where(e => e.EsFoto == false && e.Borrado == false) // Filtrar evoluciones válidas
                        .SelectMany(e => e.Registros) // Seleccionar todos los registros de esas evoluciones
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r => DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) < request.FechaInicio));

                    break;
                case ComparacionTipos.Entre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        //AddCriteria(i => i.Suceso.Evoluciones
                        //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //.Select(e => e.Registro.FechaHoraEvolucion)
                        //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                        //.FirstOrDefault() != null &&

                        //    (
                        //    DateOnly.FromDateTime(i.Suceso.Evoluciones
                        //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //    .Select(e => e.Registro.FechaHoraEvolucion)
                        //    .OrderByDescending(fecha => fecha)
                        //    .FirstOrDefault().Value) >= request.FechaInicio &&

                        //    DateOnly.FromDateTime(i.Suceso.Evoluciones
                        //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //    .Select(e => e.Registro.FechaHoraEvolucion)
                        //    .OrderByDescending(fecha => fecha)
                        //    .FirstOrDefault().Value) <= request.FechaInicio
                        //    )
                        //);

                        //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) >= request.FechaInicio && DateOnly.FromDateTime(incendio.FechaCreacion) <= request.FechaFin);

                        AddCriteria(i => i.Suceso.Evoluciones
                        .Where(e => e.EsFoto == false && e.Borrado == false) // Filtrar evoluciones válidas
                        .SelectMany(e => e.Registros) // Seleccionar todos los registros de esas evoluciones
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r => 
                            DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) >= request.FechaInicio &&
                            DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) <= request.FechaInicio
                            )
                        );
                    }
                    else
                    {
                        throw new ArgumentException("Las fechas de inicio y fin deben ser proporcionadas para la comparación 'Entre'");
                    }
                    break;
                case ComparacionTipos.NoEntre:
                    if (request.FechaInicio.HasValue && request.FechaFin.HasValue)
                    {
                        //AddCriteria(i => i.Suceso.Evoluciones
                        //.Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //.Select(e => e.Registro.FechaHoraEvolucion)
                        //.OrderByDescending(fecha => fecha) // Ordena para tomar la más reciente
                        //.FirstOrDefault() != null &&

                        //    (
                        //    DateOnly.FromDateTime(i.Suceso.Evoluciones
                        //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //    .Select(e => e.Registro.FechaHoraEvolucion)
                        //    .OrderByDescending(fecha => fecha)
                        //    .FirstOrDefault().Value) < request.FechaInicio ||

                        //    DateOnly.FromDateTime(i.Suceso.Evoluciones
                        //    .Where(e => e.EsFoto == false && e.Borrado == false && e.Registro.FechaHoraEvolucion != null)
                        //    .Select(e => e.Registro.FechaHoraEvolucion)
                        //    .OrderByDescending(fecha => fecha)
                        //    .FirstOrDefault().Value) > request.FechaInicio
                        //    )
                        //);

                        //AddCriteria(incendio => DateOnly.FromDateTime(incendio.FechaCreacion) < request.FechaInicio || DateOnly.FromDateTime(incendio.FechaCreacion) > request.FechaFin);

                        AddCriteria(i => i.Suceso.Evoluciones
                        .Where(e => e.EsFoto == false && e.Borrado == false) // Filtrar evoluciones válidas
                        .SelectMany(e => e.Registros) // Seleccionar todos los registros de esas evoluciones
                        .Where(r => r.FechaHoraEvolucion != null && r.Borrado == false) // Excluir registros borrados lógicamente
                        .OrderByDescending(r => r.FechaCreacion) // Ordenar por fecha más reciente
                        .Take(1) // Solo tomar el más reciente
                        .Any(r =>
                            DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) < request.FechaInicio &&
                            DateOnly.FromDateTime(r.FechaHoraEvolucion.Value) > request.FechaInicio
                            )
                        );
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




        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
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
                case "estadosucesoasc":
                    AddOrderBy(i => i.IdEstadoSuceso);
                    break;
                case "estadosucesodesc":
                    AddOrderByDescending(i => i.IdEstadoSuceso);
                    break;
                default:
                    AddOrderBy(i => i.FechaInicio); // Orden por defecto
                    break;
            }
        }
    }
}
