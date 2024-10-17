using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Services;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;



namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.CreateEvoluciones
{

    public class CreateEvolucionCommandHandler : IRequestHandler<CreateEvolucionCommand, CreateEvolucionResponse>
    {
        private readonly ILogger<CreateEvolucionCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeometryValidator _geometryValidator;
        private readonly ICoordinateTransformationService _coordinateTransformationService;
        private readonly IEvolucionService _evolucionService;


        public CreateEvolucionCommandHandler(
            ILogger<CreateEvolucionCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IGeometryValidator geometryValidator,
            ICoordinateTransformationService coordinateTransformationService,
            IEvolucionService evolucionService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _geometryValidator = geometryValidator;
            _coordinateTransformationService = coordinateTransformationService;
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

            var entradaSalida = await _unitOfWork.Repository<EntradaSalida>().GetByIdAsync(request.IdEntradaSalida);
            if (entradaSalida is null)
            {
                _logger.LogWarning($"request.IdEntradaSalida: {request.IdEntradaSalida}, no encontrado");
                throw new NotFoundException(nameof(EntradaSalida), request.IdEntradaSalida);
            }

            var medio = await _unitOfWork.Repository<Medio>().GetByIdAsync(request.IdMedio);
            if (medio is null)
            {
                _logger.LogWarning($"request.IdMedio: {request.IdMedio}, no encontrado");
                throw new NotFoundException(nameof(Medio), request.IdMedio);
            }


            var estadoIncendio = await _unitOfWork.Repository<EstadoIncendio>().GetByIdAsync(request.IdEstadoIncendio);
            if (estadoIncendio is null)
            {
                _logger.LogWarning($"request.IdEstadoIncendio: {request.IdEstadoIncendio}, no encontrado");
                throw new NotFoundException(nameof(EstadoIncendio), request.IdEstadoIncendio);
            }

            var provincia = await _unitOfWork.Repository<Provincia>().GetByIdAsync(request.IdProvinciaAfectada);
            if (provincia is null)
            {
                _logger.LogWarning($"request.IdProvinciaAfectada: {request.IdProvinciaAfectada}, no encontrado");
                throw new NotFoundException(nameof(Provincia), request.IdProvinciaAfectada);
            }

            var municipio = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipioAfectado);
            if (municipio is null)
            {
                _logger.LogWarning($"request.IdMunicipioAfectado: {request.IdMunicipioAfectado}, no encontrado");
                throw new NotFoundException(nameof(NivelGravedad), request.IdMunicipioAfectado);
            }

            // var tecnico = await _unitOfWork.Repository<ApplicationUser>().GetByIdAsync(request.IdTecnico);
            //if (tecnico is null)
            //{
            //    _logger.LogWarning($"request.IdTecnico: {request.IdTecnico}, no encontrado");
            //    throw new NotFoundException(nameof(ApplicationUser), request.IdTecnico);
            //}

            var entidadMenor = await _unitOfWork.Repository<EntidadMenor>().GetByIdAsync(request.IdEntidadMenor);
            if (entidadMenor is null)
            {
                _logger.LogWarning($"request.IdEntidadMenor: {request.IdEntidadMenor}, no encontrado");
                throw new NotFoundException(nameof(EntidadMenor), request.IdEntidadMenor);
            }

            var tipoRegistro = await _unitOfWork.Repository<TipoRegistro>().GetByIdAsync(request.IdTipoRegistro);
            if (tipoRegistro is null)
            {
                _logger.LogWarning($"request.IdTipoRegistro: {request.IdTipoRegistro}, no encontrado");
                throw new NotFoundException(nameof(TipoRegistro), request.IdTipoRegistro);
            }

            if (request.GeoPosicionAreaAfectada != null && !_geometryValidator.IsGeometryValidAndInEPSG4326(request.GeoPosicionAreaAfectada))
            {
                ValidationFailure validationFailure = new ValidationFailure();
                validationFailure.ErrorMessage = "No es una geometria valida o no tiene el EPS4326";

                _logger.LogWarning($"{validationFailure}, geometria -> {request.GeoPosicionAreaAfectada}");
                throw new ValidationException(new List<ValidationFailure> { validationFailure });
            }

            
            await _evolucionService.ComprobacionEvolucionProcedenciaDestinos(request.EvolucionProcedenciaDestinos);
            var evolucion = await _evolucionService.CrearNuevaEvolucion(request);

            if (request.EvolucionProcedenciaDestinos != null)
            {
               await _evolucionService.CrearEvolucioneProcedenciaDestinos(evolucion.Id, request.EvolucionProcedenciaDestinos);
            }
            
            await _evolucionService.CambiarEstadoSucesoIncendioEvolucion(evolucion.IdEstadoIncendio, evolucion.IdIncendio);

            _logger.LogInformation(nameof(CreateEvolucionCommandHandler) + " - END");
            return new CreateEvolucionResponse { Id = evolucion.Id };
        }
    }
}
