using DGPCE.Sigemad.Domain.Modelos;


namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;



public class DireccionCoordinacionEmergenciaActiveByIdSpecification : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaActiveByIdSpecification(DireccionCoordinacionEmergenciaSpecificationParams request)
       : base(DireccionCoordinacionEmergencia =>
         (!request.Id.HasValue || DireccionCoordinacionEmergencia.Id == request.Id) &&
        (!request.IdIncendio.HasValue || DireccionCoordinacionEmergencia.IdIncendio == request.IdIncendio) &&
        (!request.IdTipoDireccionEmergencia.HasValue || DireccionCoordinacionEmergencia.IdTipoDireccionEmergencia == request.IdTipoDireccionEmergencia) &&
        (!request.IdProvinciaCECOPI.HasValue || DireccionCoordinacionEmergencia.IdProvinciaCECOPI == request.IdProvinciaCECOPI) &&
        (!request.IdMunicipioCECOPI.HasValue || DireccionCoordinacionEmergencia.IdMunicipioCECOPI == request.IdMunicipioCECOPI) &&
        (!request.IdProvinciaPMA.HasValue || DireccionCoordinacionEmergencia.IdProvinciaPMA == request.IdProvinciaPMA) &&
        (!request.IdMunicipioPMA.HasValue || DireccionCoordinacionEmergencia.IdMunicipioPMA == request.IdMunicipioPMA) &&
        (DireccionCoordinacionEmergencia.Borrado == false))
    {
        AddInclude(e => e.ActivacionPlanEmergencia);
    }
}
