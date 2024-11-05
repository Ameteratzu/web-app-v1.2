using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Delete;

public class DeleteDireccionCoordinacionEmergenciaCommandHandler : IRequestHandler<DeleteDireccionCoordinacionEmergenciaCommand>
{
    private readonly ILogger<DeleteDireccionCoordinacionEmergenciaCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteDireccionCoordinacionEmergenciaCommandHandler(
        ILogger<DeleteDireccionCoordinacionEmergenciaCommand> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteDireccionCoordinacionEmergenciaCommand request, CancellationToken cancellationToken)
    {
        var direccionCoordinacionEmergenciaToDelete = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetByIdAsync(request.Id);
        if (direccionCoordinacionEmergenciaToDelete is null)
        {
            _logger.LogWarning($"la DireccionCoordinacionEmergencia con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(Incendio), request.Id);
        }

        direccionCoordinacionEmergenciaToDelete.Borrado = true;
        direccionCoordinacionEmergenciaToDelete.FechaEliminacion = DateTime.Now;
        _unitOfWork.Repository<DireccionCoordinacionEmergencia>().UpdateEntity(direccionCoordinacionEmergenciaToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"La DireccionCoordinacionEmergencia con id: {request.Id}, se actualizo estado de borrado con éxito");

        return Unit.Value;
    }
}