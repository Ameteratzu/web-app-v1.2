using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;

public class CreateIncendioCommandHandler : IRequestHandler<CreateIncendioCommand, CreateIncendioResponse>
{
    private readonly ILogger<CreateIncendioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeometryValidator _geometryValidator;
    private readonly ICoordinateTransformationService _coordinateTransformationService;

    public CreateIncendioCommandHandler(
        ILogger<CreateIncendioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IGeometryValidator geometryValidator,
        ICoordinateTransformationService coordinateTransformationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _geometryValidator = geometryValidator;
        _coordinateTransformationService = coordinateTransformationService;
    }

    public async Task<CreateIncendioResponse> Handle(CreateIncendioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateIncendioCommandHandler) + " - BEGIN");

        var territorio = await _unitOfWork.Repository<Territorio>().GetByIdAsync(request.IdTerritorio);
        if (territorio is null)
        {
            _logger.LogWarning($"request.IdTerritorio: {request.IdTerritorio}, no encontrado");
            throw new NotFoundException(nameof(Territorio), request.IdTerritorio);
        }

        var pais = await _unitOfWork.Repository<Pais>().GetByIdAsync(request.IdPais);
        if (pais is null)
        {
            _logger.LogWarning($"request.IdPais: {request.IdPais}, no encontrado");
            throw new NotFoundException(nameof(Territorio), request.IdPais);
        }

        var provincia = await _unitOfWork.Repository<Provincia>().GetByIdAsync(request.IdProvincia);
        if (provincia is null)
        {
            _logger.LogWarning($"request.IdProvincia: {request.IdProvincia}, no encontrado");
            throw new NotFoundException(nameof(Provincia), request.IdProvincia);
        }

        var municipio = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipio);
        if (municipio is null)
        {
            _logger.LogWarning($"request.IdMunicipio: {request.IdMunicipio}, no encontrado");
            throw new NotFoundException(nameof(Municipio), request.IdMunicipio);
        }

        var tipoSuceso = await _unitOfWork.Repository<TipoSuceso>().GetByIdAsync(request.IdTipoSuceso);
        if (tipoSuceso is null)
        {
            _logger.LogWarning($"request.IdTipoSuceso: {request.IdTipoSuceso}, no encontrado");
            throw new NotFoundException(nameof(TipoSuceso), request.IdTipoSuceso);
        }

        var claseSuceso = await _unitOfWork.Repository<ClaseSuceso>().GetByIdAsync(request.IdClaseSuceso);
        if (claseSuceso is null)
        {
            _logger.LogWarning($"request.IdTipoSuceso: {request.IdClaseSuceso}, no encontrado");
            throw new NotFoundException(nameof(ClaseSuceso), request.IdClaseSuceso);
        }

        var estado = await _unitOfWork.Repository<EstadoSuceso>().GetByIdAsync(request.IdEstadoSuceso);
        if (estado is null)
        {
            _logger.LogWarning($"request.IdEstado: {request.IdEstadoSuceso}, no encontrado");
            throw new NotFoundException(nameof(EstadoIncendio), request.IdEstadoSuceso);
        }

        if (!_geometryValidator.IsGeometryValidAndInEPSG4326(request.GeoPosicion))
        {
            ValidationFailure validationFailure = new ValidationFailure();
            validationFailure.ErrorMessage = "No es una geometria valida o no tiene el EPS4326";

            _logger.LogWarning($"{validationFailure}, geometria -> {request.GeoPosicion}");
            throw new ValidationException(new List<ValidationFailure> { validationFailure });
        }

        var suceso = new Suceso
        {
            IdTipo = request.IdTipoSuceso
        };

        var (utmX, utmY, huso) = _coordinateTransformationService.ConvertToUTM(request.GeoPosicion);

        var incendio = new Incendio
        {
            Suceso = suceso,
            IdTerritorio = request.IdTerritorio,
            IdPais = request.IdPais,
            IdProvincia = request.IdProvincia,
            IdMunicipio = request.IdMunicipio,
            IdClaseSuceso = request.IdClaseSuceso,
            IdEstadoSuceso = request.IdEstadoSuceso,
            Denominacion = request.Denominacion,
            Comentarios = request.Comentarios,
            FechaInicio = request.FechaInicio,
            RutaMapaRiesgo = request.RutaMapaRiesgo,
            GeoPosicion = request.GeoPosicion,
            UtmX = (decimal?)utmX,
            UtmY = (decimal?)utmY,
            Huso = huso
        };

        _unitOfWork.Repository<Incendio>().AddEntity(incendio);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo incendio");
        }

        _logger.LogInformation($"El incendio {incendio.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateIncendioCommandHandler) + " - END");
        return new CreateIncendioResponse { Id = incendio.Id } ;
    }
}
