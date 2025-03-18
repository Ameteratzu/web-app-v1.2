using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorByIdMunicipioList;


public class GetEntidadMenorByIdMunicipioListQueryHandler : IRequestHandler<GetEntidadMenorByIdMunicipioListQuery, IReadOnlyList<EntidadMenorVm>>
{
    private readonly ILogger<GetEntidadMenorByIdMunicipioListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetEntidadMenorByIdMunicipioListQueryHandler(IUnitOfWork unitOfWork, ILogger<GetEntidadMenorByIdMunicipioListQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IReadOnlyList<EntidadMenorVm>> Handle(GetEntidadMenorByIdMunicipioListQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<EntidadMenorVm> entidadMenorVms = await _unitOfWork.Repository<EntidadMenor>().GetAsync
            (
                predicate: e => e.IdMunicipio == request.IdMunicipio && e.Borrado == false,
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
