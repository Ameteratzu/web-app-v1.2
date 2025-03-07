using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionCoordinacionEmergenciaWithCoordinacionCecopis : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaWithCoordinacionCecopis(DireccionCoordinacionEmergenciaParams @params)
        : base(d =>
        (!@params.Id.HasValue || d.Id == @params.Id) &&
        (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
         d.Borrado == false)
    {
        AddInclude(d => d.CoordinacionesCecopi);
        AddInclude("CoordinacionesCecopi.Provincia");
        AddInclude("CoordinacionesCecopi.Municipio");
    }
}
