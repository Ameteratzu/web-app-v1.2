
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones
{
    public class EvolucionSpecification : BaseSpecification<Evolucion>
    {
        public EvolucionSpecification(EvolucionSpecificationParams request)
         : base(Evolucion =>
        (!request.Id.HasValue || Evolucion.Id == request.Id) &&
        (!request.IdSuceso.HasValue || Evolucion.IdSuceso == request.IdSuceso) &&
        (Evolucion.Borrado == false)
       )
        {
            AddInclude(i => i.Registro);
            AddInclude(i => i.Registro.Medio);
            AddInclude(i => i.Registro.EntradaSalida);
            AddInclude(i => i.Registro.ProcedenciaDestinos);
            AddInclude("Registro.ProcedenciaDestinos.ProcedenciaDestino");

            AddInclude(i => i.DatoPrincipal);

            AddInclude(i => i.Parametros);
            AddInclude("Parametros.PlanEmergencia");
            AddInclude("Parametros.FaseEmergencia");
            AddInclude("Parametros.PlanSituacion");
            AddInclude("Parametros.SituacionEquivalente");
            AddInclude("Parametros.EstadoIncendio");

            AddInclude(e => e.AreaAfectadas.Where(area => !area.Borrado));
            AddInclude("AreaAfectadas.Municipio");
            AddInclude("AreaAfectadas.Provincia");
            AddInclude("AreaAfectadas.EntidadMenor");

            AddInclude(e => e.Impactos.Where(consecuencia => !consecuencia.Borrado));
            AddInclude("Impactos.ImpactoClasificado");
            AddInclude("Impactos.TipoDanio");

            AddInclude(e => e.IntervencionMedios.Where(intervencion => !intervencion.Borrado));
            AddInclude("IntervencionMedios.CaracterMedio");
            AddInclude("IntervencionMedios.TitularidadMedio");
            AddInclude("IntervencionMedios.Municipio");
            AddInclude("IntervencionMedios.Provincia");
            AddInclude("IntervencionMedios.DetalleIntervencionMedios");
            AddInclude("IntervencionMedios.DetalleIntervencionMedios.MediosCapacidad");
        }
}
}