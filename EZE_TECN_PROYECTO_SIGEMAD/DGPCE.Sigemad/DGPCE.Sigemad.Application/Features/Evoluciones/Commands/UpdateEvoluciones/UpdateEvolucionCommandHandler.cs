using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Services;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
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

            var evolucionToUpdate = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(new UpdateEvolucionSpecification(request.Id));


            if (evolucionToUpdate == null)
            {
                _logger.LogWarning($"No se encontro evolución con id: {request.Id}");
                throw new NotFoundException(nameof(Evolucion), request.Id);
            }

            var incendio = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.IdIncendio);
            if (incendio is null)
            {
                _logger.LogWarning($"request.IdIncendio: {request.IdIncendio}, no encontrado");
                throw new NotFoundException(nameof(Incendio), request.IdIncendio);
            }

            if (request.IdEntradaSalida != null)
            {
                var entradaSalida = await _unitOfWork.Repository<EntradaSalida>().GetByIdAsync((int)request.IdEntradaSalida);
                if (entradaSalida is null)
                {
                    _logger.LogWarning($"request.IdEntradaSalida: {request.IdEntradaSalida}, no encontrado");
                    throw new NotFoundException(nameof(EntradaSalida), request.IdEntradaSalida);
                }
            }

            if (request.IdTipoRegistro != null)
            {
                var tipoRegistro = await _unitOfWork.Repository<TipoRegistro>().GetByIdAsync((int)request.IdTipoRegistro);
                if (tipoRegistro is null)
                {
                    _logger.LogWarning($"request.IdTipoRegistro: {request.IdTipoRegistro}, no encontrado");
                    throw new NotFoundException(nameof(TipoRegistro), request.IdTipoRegistro);
                }
            }

            if (request.IdMedio != null)
            {
                var medio = await _unitOfWork.Repository<Medio>().GetByIdAsync((int)request.IdMedio);
                if (medio is null)
                {
                    _logger.LogWarning($"request.IdMedio: {request.IdMedio}, no encontrado");
                    throw new NotFoundException(nameof(Medio), request.IdMedio);
                }
            }
             await _evolucionService.ComprobacionEvolucionProcedenciaDestinos(request.EvolucionProcedenciaDestinos);
            _mapper.Map(request, evolucionToUpdate, typeof(UpdateEvolucionCommand), typeof(Evolucion));

            _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucionToUpdate);
            await _unitOfWork.Complete();
            //await _evolucionService.CambiarEstadoIncendioDesdeEstadoEvolucion(evolucionToUpdate.IdEstadoEvolucion, evolucionToUpdate.IdIncendio);

            _logger.LogInformation($"Se actualizo correctamente la evolución con id: {request.Id}");

            return Unit.Value;
        }
    }
}
