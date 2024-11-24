using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;

public class DireccionCoordinacionEmergenciaActiveByIdSpecification : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaActiveByIdSpecification(DireccionCoordinacionEmergenciaSpecificationParams request)
       : base(DireccionCoordinacionEmergencia =>
         (!request.Id.HasValue || DireccionCoordinacionEmergencia.Id == request.Id) &&
        (!request.IdIncendio.HasValue || DireccionCoordinacionEmergencia.IdIncendio == request.IdIncendio) &&
        (DireccionCoordinacionEmergencia.Borrado == false))
    {
        if (request.Id.HasValue)
        {
            AddInclude(d => d.Direcciones);
            
            AddInclude(d => d.CoordinacionesCecopi);
            //AddInclude(d => d.CoordinacionesCecopi.Select(d => d.Provincia));
            //AddInclude(d => d.CoordinacionesCecopi.Select(d => d.Municipio));

            AddInclude(d => d.CoordinacionesPMA);
            //AddInclude(d => d.CoordinacionesPMA.Select(d => d.Provincia));
            //AddInclude(d => d.CoordinacionesPMA.Select(d => d.Municipio));
        }
    }
}
