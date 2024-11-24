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
    }
}
