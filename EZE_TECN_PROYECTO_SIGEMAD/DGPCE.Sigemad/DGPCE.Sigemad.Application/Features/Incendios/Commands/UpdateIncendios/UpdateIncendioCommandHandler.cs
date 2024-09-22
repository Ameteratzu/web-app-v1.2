using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;

public class UpdateIncendioCommandHandler : IRequestHandler<UpdateIncendioCommand>
{
    private readonly ILogger<UpdateIncendioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateIncendioCommandHandler(
        ILogger<UpdateIncendioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateIncendioCommand request, CancellationToken cancellationToken)
    {
        var incendioToUpdate = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.Id);

        if (incendioToUpdate == null)
        {
            _logger.LogWarning($"No se encontro incendio con id: {request.Id}");
            throw new NotFoundException(nameof(Incendio), request.Id);
        }

        _mapper.Map(request, incendioToUpdate, typeof(UpdateIncendioCommand), typeof(Incendio));

        _unitOfWork.Repository<Incendio>().UpdateEntity(incendioToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el incendio con id: {request.Id}");

        return Unit.Value;
    }
}
