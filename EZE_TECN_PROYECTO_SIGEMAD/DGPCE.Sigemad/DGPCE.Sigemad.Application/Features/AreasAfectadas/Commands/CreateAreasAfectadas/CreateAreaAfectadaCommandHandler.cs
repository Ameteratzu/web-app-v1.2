using AutoMapper;
using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.CreateAreasAfectadas;
public class CreateAreaAfectadaCommandHandler : IRequestHandler<CreateAreaAfectadaCommand, CreateAreaAfectadaResponse>
{
    private readonly ILogger<CreateIncendioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeometryValidator _geometryValidator;
    private readonly ICoordinateTransformationService _coordinateTransformationService;
    private readonly IMapper _mapper;

    public CreateAreaAfectadaCommandHandler(
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


    public async Task<CreateAreaAfectadaResponse> Handle(CreateAreaAfectadaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateAreaAfectadaCommandHandler) + " - BEGIN");

        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdAsync(request.IdEvolucion);
        if (evolucion is null)
        {
            _logger.LogWarning($"request.IdEvolucion: {request.IdEvolucion}, no encontrado");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
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

        if (!_geometryValidator.IsGeometryValidAndInEPSG4326(request.GeoPosicion))
        {
            ValidationFailure validationFailure = new ValidationFailure();
            validationFailure.ErrorMessage = "No es una geometria valida o no tiene el EPS4326";

            _logger.LogWarning($"{validationFailure}, geometria -> {request.GeoPosicion}");
            throw new ValidationException(new List<ValidationFailure> { validationFailure });
        }

        var areaAfectadaEntity = _mapper.Map<AreaAfectada>(request);

        _unitOfWork.Repository<AreaAfectada>().AddEntity(areaAfectadaEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nueva area afectada");
        }

        _logger.LogInformation($"El area afectada {areaAfectadaEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateAreaAfectadaCommandHandler) + " - END");
        return new CreateAreaAfectadaResponse { Id = areaAfectadaEntity.Id };
    }


}
