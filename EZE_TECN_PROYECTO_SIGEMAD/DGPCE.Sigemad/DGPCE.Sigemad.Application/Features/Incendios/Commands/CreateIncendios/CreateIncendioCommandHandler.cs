using AutoMapper;
using DGPCE.Sigemad.Application.Constants;
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
    private readonly IMapper _mapper;

    public CreateIncendioCommandHandler(
        ILogger<CreateIncendioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IGeometryValidator geometryValidator,
        ICoordinateTransformationService coordinateTransformationService,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _geometryValidator = geometryValidator;
        _coordinateTransformationService = coordinateTransformationService;
        _mapper = mapper;
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
            throw new NotFoundException(nameof(EstadoSuceso), request.IdEstadoSuceso);
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
            IdTipo = 1
        };

        var (utmX, utmY, huso) = _coordinateTransformationService.ConvertToUTM(request.GeoPosicion);


        var incendioEntity = _mapper.Map<Incendio>(request);
        incendioEntity.UtmX = (decimal?)utmX;
        incendioEntity.UtmY = (decimal?)utmY;
        incendioEntity.Huso = huso;
        incendioEntity.Suceso = suceso;


        if(request.IdTerritorio == TerritorioTipos.Nacional)
        {
            var provincia = await _unitOfWork.Repository<Provincia>().GetByIdAsync(request.IdProvincia.Value);
            if (provincia is null)
            {
                _logger.LogWarning($"request.IdProvincia: {request.IdProvincia}, no encontrado");
                throw new NotFoundException(nameof(Provincia), request.IdProvincia);
            }

            var municipio = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipio.Value);
            if (municipio is null)
            {
                _logger.LogWarning($"request.IdMunicipio: {request.IdMunicipio}, no encontrado");
                throw new NotFoundException(nameof(Municipio), request.IdMunicipio);
            }


            var incendioNacional = _mapper.Map<IncendioNacional>(request);
            incendioEntity.IncendioNacional = incendioNacional;
        } 
        else if(request.IdTerritorio == TerritorioTipos.Extranjero)
        {
            var pais = await _unitOfWork.Repository<Pais>().GetByIdAsync(request.IdPais.Value);
            if (pais is null)
            {
                _logger.LogWarning($"request.IdPais: {request.IdPais}, no encontrado");
                throw new NotFoundException(nameof(Pais), request.IdPais);
            }

            var distrito = await _unitOfWork.Repository<Distrito>().GetByIdAsync(request.IdDistrito.Value);
            if (pais is null)
            {
                _logger.LogWarning($"request.IdDistrito: {request.IdDistrito}, no encontrado");
                throw new NotFoundException(nameof(Distrito), request.IdDistrito);
            }

            var entidadMenor = await _unitOfWork.Repository<EntidadMenor>().GetByIdAsync(request.IdEntidadMenor.Value);
            if (pais is null)
            {
                _logger.LogWarning($"request.IdEntidadMenor: {request.IdEntidadMenor}, no encontrado");
                throw new NotFoundException(nameof(EntidadMenor), request.IdEntidadMenor);
            }

            var incendioExtranjero = _mapper.Map<IncendioExtranjero>(request);
            incendioEntity.IncendioExtranjero = incendioExtranjero;
        }


        _unitOfWork.Repository<Incendio>().AddEntity(incendioEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo incendio");
        }

        _logger.LogInformation($"El incendio {incendioEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateIncendioCommandHandler) + " - END");
        return new CreateIncendioResponse { Id = incendioEntity.Id } ;
    }
}
