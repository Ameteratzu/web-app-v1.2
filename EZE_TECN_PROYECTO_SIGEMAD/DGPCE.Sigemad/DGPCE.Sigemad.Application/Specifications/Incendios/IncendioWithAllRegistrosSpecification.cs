using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.Incendios;
public class IncendioWithAllRegistrosSpecification : BaseSpecification<Incendio>
{
    public IncendioWithAllRegistrosSpecification(int idIncendio)
        : base(i => i.Id == idIncendio && i.Borrado == false)
    {
        AddInclude(i => i.Evoluciones); // Incluir Datos de Evolución
        AddInclude(i => i.OtraInformaciones); // Incluir Otra Información
        AddInclude(i => i.DireccionCoordinacionEmergencias); // Incluir Dirección y Coordinación
    }
}

