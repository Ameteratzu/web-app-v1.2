
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasList;
public class GetDireccionCoordinacionEmergenciasListQuery : IRequest<IReadOnlyList<DireccionCoordinacionEmergenciaVm>>
{
}
