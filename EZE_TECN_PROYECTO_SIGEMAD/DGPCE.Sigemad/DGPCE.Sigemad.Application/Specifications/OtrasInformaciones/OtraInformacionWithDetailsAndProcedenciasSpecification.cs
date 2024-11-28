using DGPCE.Sigemad.Domain.Modelos;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class OtraInformacionWithDetailsAndProcedenciasSpecification: BaseSpecification<OtraInformacion>
{
    /*
    public Expression<Func<OtraInformacion, bool>> Criteria { get; }
    public List<Expression<Func<OtraInformacion, object>>> Includes { get; } = new();

    
    public OtraInformacionWithDetailsAndProcedenciasSpecification(int id):
        base()
    {
        // Filtro: Obtener solo la entidad con el ID proporcionado que no esté marcada como borrada
        Criteria = oi => oi.Id == id && !oi.Borrado;

        // Relación principal: Detalles
        Includes.Add(oi => oi.DetallesOtraInformacion);

        // Relación secundaria: Procedencias/Destinos dentro de los detalles
        Includes.Add(oi => oi.DetallesOtraInformacion.Select(d => d.ProcedenciasDestinos));
    }
    */
    public OtraInformacionWithDetailsAndProcedenciasSpecification(int id)
    : base(oi => oi.Id == id && !oi.Borrado) // Filtro principal
    {
        // Relación principal: Detalles
        AddInclude(oi => oi.DetallesOtraInformacion);
    }
}

