using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionCoordinacionEmergenciaWithCoordinacionPma : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaWithCoordinacionPma(DireccionCoordinacionEmergenciaParams @params)
        : base(d =>
        (!@params.Id.HasValue || d.Id == @params.Id) &&
        (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
         d.Borrado == false)
    {
        AddInclude(d => d.CoordinacionesPMA);
        AddInclude("CoordinacionesPMA.Provincia");
        AddInclude("CoordinacionesPMA.Municipio");
    }
}
