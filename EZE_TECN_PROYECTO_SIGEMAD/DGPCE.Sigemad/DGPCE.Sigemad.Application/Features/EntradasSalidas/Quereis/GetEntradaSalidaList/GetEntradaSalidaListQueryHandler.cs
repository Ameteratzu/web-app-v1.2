using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EntradasSalidas.Quereis.GetEntradaSalidaList;
public class GetEntradaSalidaListQueryHandler : IRequestHandler<GetEntradaSalidaListQuery, IReadOnlyList<EntradaSalida>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEntradaSalidaListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<EntradaSalida>> Handle(GetEntradaSalidaListQuery request, CancellationToken cancellationToken)
    {
        var entradasSalidas = await _unitOfWork.Repository<EntradaSalida>().GetAllAsync();
        return entradasSalidas;
    }
}