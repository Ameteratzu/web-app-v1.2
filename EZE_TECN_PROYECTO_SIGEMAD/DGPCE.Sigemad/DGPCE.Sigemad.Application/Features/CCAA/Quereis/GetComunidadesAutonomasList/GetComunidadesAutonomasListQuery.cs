using DGPCE.Sigemad.Application.Features.Alertas.Queries.GetEstadosAlertasList;
using DGPCE.Sigemad.Application.Features.CCAA.Quereis.Vms;
using DGPCE.Sigemad.Application.Features.EstadosAlertas.Queries.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CCAA.Quereis.GetComunidadesAutonomasList
{
    public class GetComunidadesAutonomasListQuery :IRequest<IReadOnlyList<Ccaa>>
    {
    }
}
