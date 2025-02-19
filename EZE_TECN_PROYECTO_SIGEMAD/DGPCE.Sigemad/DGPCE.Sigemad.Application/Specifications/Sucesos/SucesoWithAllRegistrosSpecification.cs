using DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetRegistrosPorIncendio;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Sucesos;
public class SucesoWithAllRegistrosSpecification : BaseSpecification<Suceso>
{
    public SucesoWithAllRegistrosSpecification(GetRegistrosPorSucesoQuery request)
        : base(s => s.Id == request.IdSuceso && s.Borrado == false)
    {
        //AddInclude(i => i.Evoluciones.Where(dir => !dir.Borrado));
        AddInclude("Evoluciones.Registro");
        AddInclude("Evoluciones.DatoPrincipal");
        AddInclude("Evoluciones.Parametro");
        AddInclude("Evoluciones.AreaAfectadas");
        AddInclude("Evoluciones.Impactos");

        AddInclude(i => i.Documentaciones.Where(dir => !dir.Borrado));
        AddInclude("Documentaciones.DetallesDocumentacion");

        AddInclude(i => i.OtraInformaciones.Where(dir => !dir.Borrado));
        AddInclude("OtraInformaciones.DetallesOtraInformacion");

       //AddInclude(i => i.DireccionCoordinacionEmergencias.Where(dir => !dir.Borrado));
        AddInclude("DireccionCoordinacionEmergencias.Direcciones");
        AddInclude("DireccionCoordinacionEmergencias.CoordinacionesCecopi");
        AddInclude("DireccionCoordinacionEmergencias.CoordinacionesPMA");

        AddInclude(i => i.SucesoRelacionados.Where(dir => !dir.Borrado));
        AddInclude("SucesoRelacionados.DetalleSucesoRelacionados");

        AddInclude(i => i.ActuacionesRelevantes.Where(dir => !dir.Borrado));
        AddInclude("ActuacionesRelevantes.EmergenciaNacional");
        AddInclude("ActuacionesRelevantes.ActivacionPlanEmergencias");
        AddInclude("ActuacionesRelevantes.DeclaracionesZAGEP");
        AddInclude("ActuacionesRelevantes.ActivacionSistemas");
        AddInclude("ActuacionesRelevantes.ConvocatoriasCECOD");
        AddInclude("ActuacionesRelevantes.NotificacionesEmergencias");

    }
}
