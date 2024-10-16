using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Services;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.UpdateEvoluciones
{
    public class UpdateEvolucionCommandHandler : IRequestHandler<UpdateEvolucionCommand>
    {
        private readonly ILogger<UpdateEvolucionCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEvolucionService _evolucionService;

        public UpdateEvolucionCommandHandler(
            ILogger<UpdateEvolucionCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEvolucionService evolucionService
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _evolucionService = evolucionService;
        }

        public async Task<Unit> Handle(UpdateEvolucionCommand request, CancellationToken cancellationToken)
        {
            var evolucionToUpdate = await _unitOfWork.Repository<Evolucion>().GetByIdAsync(request.Id);

            if (evolucionToUpdate == null)
            {
                _logger.LogWarning($"No se encontro evolución con id: {request.Id}");
                throw new NotFoundException(nameof(Evolucion), request.Id);
            }

            _mapper.Map(request, evolucionToUpdate, typeof(UpdateEvolucionCommand), typeof(Evolucion));

            _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucionToUpdate);
            await _unitOfWork.Complete();
            //await _evolucionService.CambiarEstadoIncendioDesdeEstadoEvolucion(evolucionToUpdate.IdEstadoEvolucion, evolucionToUpdate.IdIncendio);

            _logger.LogInformation($"Se actualizo correctamente la evolución con id: {request.Id}");

            return Unit.Value;
        }
    }
}
