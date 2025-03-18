using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.CaracterMedios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis.GetCaracterMediosList;
public class GetCaracterMediosListQueryHandler : IRequestHandler<GetCaracterMediosListQuery, IReadOnlyList<CaracterMedioDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCaracterMediosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CaracterMedioDto>> Handle(GetCaracterMediosListQuery request, CancellationToken cancellationToken)
    {
        var caracterMedios = await _unitOfWork.Repository<CaracterMedio>().GetAsync(s => s.Obsoleto == false);

        IReadOnlyList<CaracterMedioDto> caracterMediosDto = await _unitOfWork.Repository<CaracterMedio>().GetAsync
            (
                predicate: s => s.Obsoleto == false,
                selector: s => new CaracterMedioDto
                {
                    Id = s.Id,
                    Descripcion = s.Descripcion,
                },
                disableTracking: true
            );

        return caracterMediosDto;

    }
}

