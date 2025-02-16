using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionCoordinacionEmergenciaWithDirecciones : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaWithDirecciones(DireccionCoordinacionEmergenciaParams @params)
        : base(d =>
        (!@params.Id.HasValue || d.Id == @params.Id) &&
        (!@params.IdSuceso.HasValue || d.IdSuceso == @params.IdSuceso) &&
         d.Borrado == false)
    {
        AddInclude(d => d.Direcciones);
        AddInclude("Direcciones.TipoDireccionEmergencia");
    }
}
