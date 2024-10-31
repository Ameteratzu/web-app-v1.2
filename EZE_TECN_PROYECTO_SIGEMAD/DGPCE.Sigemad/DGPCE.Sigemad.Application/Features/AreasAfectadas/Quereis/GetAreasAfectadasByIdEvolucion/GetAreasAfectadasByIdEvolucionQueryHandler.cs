using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Vms;
using DGPCE.Sigemad.Application.Specifications.AreasAfectadas;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Quereis.GetAreaAfectadaList;
internal class GetAreasAfectadasByIdEvolucionQueryHandler : IRequestHandler<GetAreasAfectadasByIdEvolucionQuery, IReadOnlyList<AreaAfectadaVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAreasAfectadasByIdEvolucionQueryHandler> _logger;

    public GetAreasAfectadasByIdEvolucionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetAreasAfectadasByIdEvolucionQueryHandler> logger
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IReadOnlyList<AreaAfectadaVm>> Handle(GetAreasAfectadasByIdEvolucionQuery request, CancellationToken cancellationToken)
    {
        var evolucionSpec = new EvolucionActiveByIdSpecification(request.IdEvolucion);
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);
        if (evolucion == null)
        {
            _logger.LogWarning($"No se encontro evolucion con id: {request.IdEvolucion}");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
        }

        var areaAfectadaSpec = new AreaAfectadaActiveByIdEvolucionSpecification(request.IdEvolucion);
        IReadOnlyList<AreaAfectada> areasAfectadas = await _unitOfWork.Repository<AreaAfectada>().GetAllWithSpec(areaAfectadaSpec);

        var areasAfectadasVm = _mapper.Map<IReadOnlyList<AreaAfectada>, IReadOnlyList<AreaAfectadaVm>>(areasAfectadas);
        return areasAfectadasVm;
    }


}
