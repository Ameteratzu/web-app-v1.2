using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
public class DireccionCoordinacionEmergenciaWithFilteredData : BaseSpecification<DireccionCoordinacionEmergencia>
{
    public DireccionCoordinacionEmergenciaWithFilteredData(int id, List<int> idsDirecciones, List<int> idsCecopi, List<int> idsPma)
        : base(d => d.Id == id && d.Borrado == false)
    {
        if (idsDirecciones.Any())
        {
            AddInclude(d => d.Direcciones.Where(dir => idsDirecciones.Contains(dir.Id) && !dir.Borrado));
            AddInclude("Direcciones.TipoDireccionEmergencia");
        }

        if (idsCecopi.Any())
        {
            AddInclude(d => d.CoordinacionesCecopi.Where(cecopi => idsCecopi.Contains(cecopi.Id) && !cecopi.Borrado));
            AddInclude("CoordinacionesCecopi.Provincia");
            AddInclude("CoordinacionesCecopi.Municipio");
        }

        if (idsPma.Any())
        {
            AddInclude(d => d.CoordinacionesPMA.Where(pma => idsPma.Contains(pma.Id) && !pma.Borrado));
            AddInclude("CoordinacionesPMA.Provincia");
            AddInclude("CoordinacionesPMA.Municipio");
        }
    }
}
