using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Alertas.Commands.DeleteAlertas
{
    public class DeleteAlertaCommandHandler : IRequestHandler<DeleteAlertaCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteAlertaCommandHandler> _logger;

        public DeleteAlertaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteAlertaCommandHandler> logger)
        {
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAlertaCommand request, CancellationToken cancellationToken)
        {
            var alertaToDelete = await _unitOfWork.Repository<Alerta>().GetByIdAsync(request.Id);
            if (alertaToDelete == null)
            {
                _logger.LogError($"La  alerta con id:{request.Id}, no existe en la base da datos");
                throw new NotFoundException(nameof(Alerta), request.Id);
            }

            //await _streamerRepository.DeleteAsync(streamerToDelete);
            _unitOfWork.Repository<Alerta>().DeleteEntity(alertaToDelete);

            await _unitOfWork.Complete();

            _logger.LogInformation($"La alerta con id: {request.Id}, fue eliminado con exito");

            return Unit.Value;
        }
    }
}
