using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Alertas.Commands.CreateAlertas
{
    public class CreateAlertaCommandHandler : IRequestHandler<CreateAlertaCommand, int>
    {
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAlertaCommandHandler> _logger;

        public CreateAlertaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateAlertaCommandHandler> logger)
        {
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateAlertaCommand request, CancellationToken cancellationToken)
        {
            var alertaEntity = _mapper.Map<Alerta>(request);
            //var newStreamer = await _streamerRepository.AddAsync(streamerEntity);

            _unitOfWork.Repository<Alerta>().AddEntity(alertaEntity);

            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                throw new Exception($"No se pudo insertar una nueva alerta");
            }

            _logger.LogInformation($"La alerta {alertaEntity.Id} fue creada correctamente");


            return alertaEntity.Id;
        }
    }
}
