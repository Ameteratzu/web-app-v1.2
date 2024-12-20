using DGPCE.Sigemad.Application.Features.Fases.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Fases.Queries;
public class GetFasesEmergenciaListByIdPlanEmergenciaQuery : IRequest<IReadOnlyList<FaseEmergenciaVm>>
{
    public int? IdPlanEmergencia { get; set; }

    public GetFasesEmergenciaListByIdPlanEmergenciaQuery(int? IdPlanEmergencia)
    {
        this.IdPlanEmergencia = IdPlanEmergencia;
    }
}
