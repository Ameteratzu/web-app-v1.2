using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.DestinosMedios.Queries.GetDestinoMediosList;
public class GetDestinoMediosListQueryHandler : IRequestHandler<GetDestinoMediosListQuery, IReadOnlyList<DestinoMedioDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDestinoMediosListQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DestinoMedioDto>> Handle(GetDestinoMediosListQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.Repository<DestinoMedio>().GetAllAsync();
        return _mapper.Map<IReadOnlyList<DestinoMedioDto>>(lista);
    }
}
