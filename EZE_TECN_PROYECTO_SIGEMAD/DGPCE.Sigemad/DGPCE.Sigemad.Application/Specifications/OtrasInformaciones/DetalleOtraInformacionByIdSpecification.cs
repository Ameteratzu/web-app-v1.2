using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class DetalleOtraInformacionByIdSpecification : BaseSpecification<DetalleOtraInformacion>
{
    public DetalleOtraInformacionByIdSpecification(int idOtraInformacion)
        : base(i => i.IdOtraInformacion == idOtraInformacion)
    {
        AddInclude(i => i.ProcedenciasDestinos);
    }
}
