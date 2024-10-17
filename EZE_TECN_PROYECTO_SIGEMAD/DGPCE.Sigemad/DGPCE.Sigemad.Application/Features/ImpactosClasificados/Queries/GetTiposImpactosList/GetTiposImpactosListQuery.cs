using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetTiposImpactosList;
public class GetTiposImpactosListQuery : IRequest<IReadOnlyList<string>>
{
}
