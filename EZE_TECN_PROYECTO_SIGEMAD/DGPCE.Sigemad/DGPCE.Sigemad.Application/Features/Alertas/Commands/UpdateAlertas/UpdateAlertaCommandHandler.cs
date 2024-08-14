using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Alertas.Commands.UpdateAlertas
{
    public class UpdateAlertaCommandHandler : IRequestHandler<UpdateAlertaCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateAlertaCommandHandler> _logger;

        public UpdateAlertaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateAlertaCommandHandler> logger)
        {
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateAlertaCommand request, CancellationToken cancellationToken)
        {
            //var streamerToUpdate =  await  _streamerRepository.GetByIdAsync(request.Id);
            var alertaToUpdate = await _unitOfWork.Repository<Alerta>().GetByIdAsync(request.Id);

            if (alertaToUpdate == null)
            {
                _logger.LogError($"No se encontro la alerta con id: {request.Id}");
                throw new NotFoundException(nameof(Alerta), request.Id);
            }

            _mapper.Map(request, alertaToUpdate, typeof(UpdateAlertaCommand), typeof(Alerta));



            //await _streamerRepository.UpdateAsync(streamerToUpdate);

            _unitOfWork.Repository<Alerta>().UpdateEntity(alertaToUpdate);

            await _unitOfWork.Complete();

            _logger.LogInformation($"La operacion se realizo correctamente para la alerta con id: {request.Id}");

            return Unit.Value;
        }
    }
}
