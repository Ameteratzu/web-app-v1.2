using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucionById;


public class GetEvolucionByIdQueryHandler : IRequestHandler<GetEvolucionByIdQuery, EvolucionDto>
{
    private readonly ILogger<GetEvolucionByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEvolucionByIdQueryHandler(
        ILogger<GetEvolucionByIdQueryHandler> logger,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EvolucionDto> Handle(GetEvolucionByIdQuery request, CancellationToken cancellationToken)
    {
        var evolucion = await _unitOfWork.Repository<Evolucion>()
        .GetByIdWithSpec(new DetalleEvolucionById(request.Id));

        if (evolucion == null)
        {
            _logger.LogWarning($"No se encontro evolucion con id: {request.Id}");
            throw new NotFoundException(nameof(Evolucion), request.Id);
        }

        var evolucionDto = _mapper.Map<Evolucion, EvolucionDto>(evolucion);
        return evolucionDto;
    }
}

