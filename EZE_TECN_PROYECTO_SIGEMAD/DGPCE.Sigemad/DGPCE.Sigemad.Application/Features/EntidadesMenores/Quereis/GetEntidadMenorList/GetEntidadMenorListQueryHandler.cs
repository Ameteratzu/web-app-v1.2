using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorList;

public class GetEntidadMenorListQueryHandler : IRequestHandler<GetEntidadMenorListQuery, IReadOnlyList<EntidadMenorVm>>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetEntidadMenorListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<EntidadMenorVm>> Handle(GetEntidadMenorListQuery request, CancellationToken cancellationToken)
    {

        IReadOnlyList<EntidadMenorVm> entidadMenorVms = await _unitOfWork.Repository<EntidadMenor>().GetAsync
            (
                predicate: e => e.Borrado == false,
                selector: e => new EntidadMenorVm
                {
                    Id = e.Id,
                    Descripcion = e.Descripcion,
                    IdMunicipio = e.Municipio.Id,
                    Huso = e.Huso,
                    GeoPosicion = e.GeoPosicion,
                    UtmX = e.UtmX,
                    UtmY = e.UtmY,
                },
                orderBy: e => e.OrderBy(e => e.Descripcion),
                disableTracking: true
            );


        return entidadMenorVms;

    }
}