
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones
{
    public class EvolucionSpecification : BaseSpecification<Evolucion>
    {

        public EvolucionSpecification(EvolucionSpecificationParams request)
         : base(Evolucion =>
        (!request.Id.HasValue || Evolucion.Id == request.Id) &&
        (!request.IdIncendio.HasValue || Evolucion.IdIncendio == request.IdIncendio) &&
        (!request.IdEntradaSalida.HasValue || Evolucion.IdEntradaSalida == request.IdEntradaSalida) &&
        (!request.IdMedio.HasValue || Evolucion.IdMedio == request.IdMedio) &&
        (!request.IdTipoRegistro.HasValue || Evolucion.IdTipoRegistro == request.IdTipoRegistro) &&
        (!request.IdSituacionOperativa.HasValue || Evolucion.IdSituacionOperativa == request.IdSituacionOperativa) &&
        (Evolucion.Borrado == false)
       )
        {
            AddInclude(i => i.EvolucionProcedenciaDestinos);
        }

    }
}
