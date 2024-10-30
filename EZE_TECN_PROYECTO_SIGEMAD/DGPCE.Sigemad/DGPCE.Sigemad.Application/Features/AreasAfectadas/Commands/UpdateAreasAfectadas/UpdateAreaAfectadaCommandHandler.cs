using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
internal class UpdateAreaAfectadaCommandHandler : IRequestHandler<UpdateAreaAfectadaCommand>
{
    private readonly ILogger<UpdateAreaAfectadaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAreaAfectadaCommandHandler(
        ILogger<UpdateAreaAfectadaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateAreaAfectadaCommand request, CancellationToken cancellationToken)
    {
        var areaAfectada = await _unitOfWork.Repository<AreaAfectada>().GetByIdAsync(request.Id);

        if (areaAfectada == null)
        {
            _logger.LogWarning($"No se encontro area afectada con id: {request.Id}");
            throw new NotFoundException(nameof(AreaAfectada), request.Id);
        }

        _mapper.Map(request, areaAfectada, typeof(UpdateAreaAfectadaCommand), typeof(AreaAfectada));

        _unitOfWork.Repository<AreaAfectada>().UpdateEntity(areaAfectada);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el area afectada con id: {request.Id}");

        return Unit.Value;
    }
}
