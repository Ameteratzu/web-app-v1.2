using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.CreateAlertas
{
    public class CreateEstadoAlertaCommandHandler : IRequestHandler<CreateEstadoAlertaCommand, int>
    {
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEstadoAlertaCommandHandler> _logger;

        public CreateEstadoAlertaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateEstadoAlertaCommandHandler> logger)
        {
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateEstadoAlertaCommand request, CancellationToken cancellationToken)
        {
            var estadoAlertaEntity = _mapper.Map<EstadoAlerta>(request);
            //var newStreamer = await _streamerRepository.AddAsync(streamerEntity);

            _unitOfWork.Repository<EstadoAlerta>().AddEntity(estadoAlertaEntity);

            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                throw new Exception($"No se pudo insertar un nuevo estado alerta");
            }

            _logger.LogInformation($"El estado de alerta {estadoAlertaEntity.Id} fue creada correctamente");


            return estadoAlertaEntity.Id;
        }
    }
}
