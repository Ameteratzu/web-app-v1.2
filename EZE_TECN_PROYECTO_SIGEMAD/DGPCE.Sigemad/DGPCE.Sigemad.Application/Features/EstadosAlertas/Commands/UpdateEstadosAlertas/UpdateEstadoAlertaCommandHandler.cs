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

namespace DGPCE.Sigemad.Application.Features.EstadosAlertas.Commands.UpdateAlertas
{
    public class UpdateEstadoAlertaCommandHandler : IRequestHandler<UpdateEstadoAlertaCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateEstadoAlertaCommandHandler> _logger;

        public UpdateEstadoAlertaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateEstadoAlertaCommandHandler> logger)
        {
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateEstadoAlertaCommand request, CancellationToken cancellationToken)
        {
            //var streamerToUpdate =  await  _streamerRepository.GetByIdAsync(request.Id);
            var estadoAlertaToUpdate = await _unitOfWork.Repository<EstadoAlerta>().GetByIdAsync(request.Id);

            if (estadoAlertaToUpdate == null)
            {
                _logger.LogError($"No se encontro el estado alerta con id: {request.Id}");
                throw new NotFoundException(nameof(Alerta), request.Id);
            }

            _mapper.Map(request, estadoAlertaToUpdate, typeof(UpdateEstadoAlertaCommand), typeof(Alerta));



            //await _streamerRepository.UpdateAsync(streamerToUpdate);

            _unitOfWork.Repository<EstadoAlerta>().UpdateEntity(estadoAlertaToUpdate);

            await _unitOfWork.Complete();

            _logger.LogInformation($"La operacion se realizo correctamente para el estado de alerta con id: {request.Id}");

            return Unit.Value;
        }
    }
}
