using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TitularidadMedios.Quereis;
public class GetTitularidadMediosListQuery : IRequest<IReadOnlyList<TitularidadMedio>>
{

}
