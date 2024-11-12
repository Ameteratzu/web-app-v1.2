using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Services;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;



namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones
{

    public class CreateEvolucionCommandHandler : IRequestHandler<CreateEvolucionCommand, CreateEvolucionResponse>
    {
        private readonly ILogger<CreateEvolucionCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeometryValidator _geometryValidator;
        private readonly IMapper _mapper;
        private readonly IEvolucionService _evolucionService;


        public CreateEvolucionCommandHandler(
            ILogger<CreateEvolucionCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IGeometryValidator geometryValidator,
            IMapper mapper,
            IEvolucionService evolucionService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _geometryValidator = geometryValidator;
            _evolucionService = evolucionService;
        }

        public async Task<CreateEvolucionResponse> Handle(CreateEvolucionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(nameof(CreateEvolucionCommandHandler) + " - BEGIN");

            var incendio = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.IdIncendio);
            if (incendio is null)
            {
                _logger.LogWarning($"request.IdIncendio: {request.IdIncendio}, no encontrado");
                throw new NotFoundException(nameof(Incendio), request.IdIncendio);
            }

            if (request.IdEntradaSalida !=null)
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


            if (request.IdEstadoIncendio != null)
            {
                var estadoIncendio = await _unitOfWork.Repository<EstadoIncendio>().GetByIdAsync((int)request.IdEstadoIncendio);
                if (estadoIncendio is null)
                {
                    _logger.LogWarning($"request.IdEstadoIncendio: {request.IdEstadoIncendio}, no encontrado");
                    throw new NotFoundException(nameof(EstadoIncendio), request.IdEstadoIncendio);
                }
            }

            if (request.IdSituacionOperativa != null)
            {
                var estadoIncendio = await _unitOfWork.Repository<SituacionOperativa>().GetByIdAsync((int)request.IdSituacionOperativa);
                if (estadoIncendio is null)
                {
                    _logger.LogWarning($"request.IdSituacionOperativa: {request.IdSituacionOperativa}, no encontrado");
                    throw new NotFoundException(nameof(SituacionOperativa), request.IdSituacionOperativa);
                }
            }

            await _evolucionService.ComprobacionEvolucionProcedenciaDestinos(request.EvolucionProcedenciaDestinos);
            var evolucion = _mapper.Map<Evolucion>(request);

            _unitOfWork.Repository<Evolucion>().AddEntity(evolucion);

            var result = await _unitOfWork.Complete();
            if (result <= 0)
            {
                throw new Exception("No se pudo insertar nueva evolución");
            }

            _logger.LogInformation($"La evolución {evolucion.Id} fue creado correctamente");

            await _evolucionService.CambiarEstadoSucesoIncendioEvolucion(evolucion.IdEstadoIncendio.Value, evolucion.IdIncendio);

            _logger.LogInformation(nameof(CreateEvolucionCommandHandler) + " - END");
            return new CreateEvolucionResponse { Id = evolucion.Id };
        }
    }
}
