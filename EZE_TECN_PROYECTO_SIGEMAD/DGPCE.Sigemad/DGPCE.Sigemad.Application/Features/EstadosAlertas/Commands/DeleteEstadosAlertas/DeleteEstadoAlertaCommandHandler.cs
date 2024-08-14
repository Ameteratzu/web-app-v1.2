using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.DeleteAlertas
{
    public class DeleteEstadoAlertaCommandHandler : IRequestHandler<DeleteEstadoAlertaCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteEstadoAlertaCommandHandler> _logger;

        public DeleteEstadoAlertaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteEstadoAlertaCommandHandler> logger)
        {
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteEstadoAlertaCommand request, CancellationToken cancellationToken)
        {
            var estadoAlertaToDelete = await _unitOfWork.Repository<EstadoAlerta>().GetByIdAsync(request.Id);
            if (estadoAlertaToDelete == null)
            {
                _logger.LogError($"El estado de alerta con id:{request.Id}, no existe en la base da datos");
                throw new NotFoundException(nameof(Alerta), request.Id);
            }

            //await _streamerRepository.DeleteAsync(streamerToDelete);
            _unitOfWork.Repository<EstadoAlerta>().DeleteEntity(estadoAlertaToDelete);

            await _unitOfWork.Complete();

            _logger.LogInformation($"El estaod de alerta con id: {request.Id}, fue eliminado con exito");

            return Unit.Value;
        }
    }
}
