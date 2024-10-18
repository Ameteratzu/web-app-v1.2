
using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Evoluciones.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvoluciones;
public class DeleteEvolucionesCommandHandler : IRequestHandler<DeleteEvolucionesCommand>
{

    private readonly ILogger<DeleteEvolucionesCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEvolucionService _evolucionService;

    public DeleteEvolucionesCommandHandler(
        ILogger<DeleteEvolucionesCommand> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEvolucionService evolucionService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _evolucionService = evolucionService;
    }

    public async Task<Unit> Handle(DeleteEvolucionesCommand request, CancellationToken cancellationToken)
    {
        await _evolucionService.EliminarEvolucion(request);

        return Unit.Value;
    }
}
