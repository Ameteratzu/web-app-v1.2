using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.DeleteAreasAfectadas;
public class DeleteAreaAfectadaCommandHandler : IRequestHandler<DeleteAreaAfectadaCommand>
{
    private readonly ILogger<DeleteAreaAfectadaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteAreaAfectadaCommandHandler(
        ILogger<DeleteAreaAfectadaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteAreaAfectadaCommand request, CancellationToken cancellationToken)
    {
        var areaAfectadaToDelete = await _unitOfWork.Repository<AreaAfectada>().GetByIdAsync(request.Id);
        if (areaAfectadaToDelete is null)
        {
            _logger.LogWarning($"El area afecatada con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(AreaAfectada), request.Id);
        }

        areaAfectadaToDelete.Borrado = true;
        areaAfectadaToDelete.FechaEliminacion = DateTime.Now;
        _unitOfWork.Repository<AreaAfectada>().UpdateEntity(areaAfectadaToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El area afectada con id: {request.Id}, se actualizo estado de borrado con éxito");

        return Unit.Value;
    }
}