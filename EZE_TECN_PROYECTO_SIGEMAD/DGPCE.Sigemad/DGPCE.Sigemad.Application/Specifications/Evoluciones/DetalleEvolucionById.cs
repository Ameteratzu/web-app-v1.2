
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones;

public class DetalleEvolucionById : BaseSpecification<Evolucion>
{
    public DetalleEvolucionById(int id)
     : base(e => e.Id == id && e.Borrado == false)
    {

        AddCriteria(i => i.Registro != null ? i.Registro.Borrado == false : true);
        AddInclude(i => i.Registro);
        AddInclude(i => i.Registro.EntradaSalida);
        AddInclude(i => i.Registro.Medio);


        AddInclude(i => i.Registro.ProcedenciaDestinos.Where(a => !a.Borrado));
        AddInclude("Registro.ProcedenciaDestinos.ProcedenciaDestino");

        AddCriteria(i => i.DatoPrincipal != null ? i.Parametro.Borrado == false : true);
        AddInclude(i => i.Parametro);
        AddInclude(i => i.Parametro.EstadoIncendio);
        AddInclude(i => i.Parametro.Fase);
        AddInclude(i => i.Parametro.SituacionEquivalente);
        AddInclude(i => i.Parametro.SituacionOperativa);


        AddCriteria(i => i.DatoPrincipal != null ? i.DatoPrincipal.Borrado == false : true);
        AddInclude(i => i.DatoPrincipal);

        AddInclude(i => i.AreaAfectadas.Where(a => !a.Borrado));
        AddInclude("AreaAfectadas.Municipio");
        AddInclude("AreaAfectadas.Provincia");
        AddInclude("AreaAfectadas.EntidadMenor");

        AddInclude(i => i.Impactos.Where(a => !a.Borrado));
        AddInclude("Impactos.ImpactoClasificado");
        AddInclude("Impactos.TipoDanio");
    }

}
