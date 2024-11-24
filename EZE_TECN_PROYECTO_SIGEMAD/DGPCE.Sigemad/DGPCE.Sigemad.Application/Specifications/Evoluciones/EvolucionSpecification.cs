﻿
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Evoluciones
{
    public class EvolucionSpecification : BaseSpecification<Evolucion>
    {
        //TODO: CORREGIR PORQUE SE CAMBIO TABLAS DE EVOLUCIONES
        public EvolucionSpecification(EvolucionSpecificationParams request)
         : base(Evolucion =>
        (!request.Id.HasValue || Evolucion.Id == request.Id) &&
        (!request.IdIncendio.HasValue || Evolucion.IdIncendio == request.IdIncendio) &&
        (Evolucion.Borrado == false)
       )
        {
            //AddInclude(i => i.EvolucionProcedenciaDestinos);
        }

    }
}
