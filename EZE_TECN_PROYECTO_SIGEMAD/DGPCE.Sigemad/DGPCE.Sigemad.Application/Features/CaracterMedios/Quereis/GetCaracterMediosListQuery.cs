using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis;
public class GetCaracterMediosListQuery : IRequest<IReadOnlyList<CaracterMedio>>
{
}
