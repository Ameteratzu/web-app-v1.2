
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
            AddInclude(i => i.Parametro);
            AddInclude(i => i.Registro);
            AddInclude(i => i.DatoPrincipal);
            AddInclude(i => i.Registro.ProcedenciaDestinos);

        }
}
}