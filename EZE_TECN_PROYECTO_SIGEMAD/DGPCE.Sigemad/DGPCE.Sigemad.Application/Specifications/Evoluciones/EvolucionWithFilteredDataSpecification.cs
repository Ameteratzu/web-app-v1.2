using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionWithFilteredDataSpecification : BaseSpecification<Evolucion>
{
    public EvolucionWithFilteredDataSpecification(
        int id,
        List<int> idsAreaAfectada,
        List<int> idsConsecuenciaActuacion,
        List<int> idsIntervencionMedio,
        bool includeRegistroParametro = false,
        bool esFoto = false)
        : base(e => e.Id == id && e.EsFoto == esFoto && e.Borrado == false)
    {
        if (includeRegistroParametro)
        {
            AddInclude(e => e.Registro);
            AddInclude(e => e.Parametro);
            AddInclude(e => e.DatoPrincipal);
        }

        if (idsAreaAfectada.Any())
        {
            AddInclude(e => e.AreaAfectadas.Where(area => idsAreaAfectada.Contains(area.Id) && !area.Borrado));
            AddInclude("AreaAfectadas.Municipio");
            AddInclude("AreaAfectadas.Provincia");
            AddInclude("AreaAfectadas.EntidadMenor");
        }

        if(idsConsecuenciaActuacion.Any())
        {
            AddInclude(e => e.Impactos.Where(consecuencia => idsConsecuenciaActuacion.Contains(consecuencia.Id) && !consecuencia.Borrado));
            AddInclude("Impactos.ImpactoClasificado");
            AddInclude("Impactos.TipoDanio");
        }

        if(idsIntervencionMedio.Any())
        {
            AddInclude(e => e.IntervencionMedios.Where(intervencion => idsIntervencionMedio.Contains(intervencion.Id) && !intervencion.Borrado));
            AddInclude("IntervencionMedios.TipoIntervencionMedio");
            AddInclude("IntervencionMedios.CaracterMedio");
            AddInclude("IntervencionMedios.ClasificacionMedio");
            AddInclude("IntervencionMedios.TitularidadMedio");
            AddInclude("IntervencionMedios.Municipio");
        }
    }
}
