using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;
public class EvolucionWithFilteredDataSpecification : BaseSpecification<Evolucion>
{
    public EvolucionWithFilteredDataSpecification(
        int id,
        List<int> idsParametro,
        List<int> idsAreaAfectada,
        List<int> idsConsecuenciaActuacion,
        List<int> idsIntervencionMedio,
        bool includeRegistro = false,
        bool includeDatoPrincipal = false,
        bool esFoto = false)
        : base(e => e.Id == id && e.EsFoto == esFoto && e.Borrado == false)
    {
        if(idsParametro.Any())
        {
            AddInclude(e => e.Parametros.Where(parametro => idsParametro.Contains(parametro.Id) && !parametro.Borrado));
            AddInclude("Parametros.PlanEmergencia");
            AddInclude("Parametros.FaseEmergencia");
            AddInclude("Parametros.PlanSituacion");
            AddInclude("Parametros.SituacionEquivalente");
            AddInclude("Parametros.EstadoIncendio");
        }

        if (includeRegistro)
        {
            AddInclude(i => i.Registro);
            AddInclude(i => i.Registro.Medio);
            AddInclude(i => i.Registro.EntradaSalida);
            AddInclude(i => i.Registro.ProcedenciaDestinos);
            AddInclude("Registro.ProcedenciaDestinos.ProcedenciaDestino");
        }

        if (includeDatoPrincipal)
        {
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
            AddInclude("IntervencionMedios.CaracterMedio");
            AddInclude("IntervencionMedios.TitularidadMedio");
            AddInclude("IntervencionMedios.Municipio");
            AddInclude("IntervencionMedios.Provincia");
            AddInclude("IntervencionMedios.DetalleIntervencionMedios");
            AddInclude("IntervencionMedios.DetalleIntervencionMedios.MediosCapacidad");
        }
    }
}
