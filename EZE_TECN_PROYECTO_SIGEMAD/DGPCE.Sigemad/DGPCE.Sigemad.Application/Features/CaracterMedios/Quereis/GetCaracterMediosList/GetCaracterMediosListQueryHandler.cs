using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.CaracterMedios;
using DGPCE.Sigemad.Application.Dtos.SituacionesEquivalentes;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis.GetCaracterMediosList;
public class GetCaracterMediosListQueryHandler : IRequestHandler<GetCaracterMediosListQuery, IReadOnlyList<CaracterMedioDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCaracterMediosListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CaracterMedioDto>> Handle(GetCaracterMediosListQuery request, CancellationToken cancellationToken)
    {
        var caracterMedios = await _unitOfWork.Repository<CaracterMedio>().GetAsync(s => s.Obsoleto == false);


        return _mapper.Map<IReadOnlyList<CaracterMedioDto>>(caracterMedios);
 
    }
}

