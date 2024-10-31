using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
internal class UpdateAreaAfectadaCommandHandler : IRequestHandler<UpdateAreaAfectadaCommand>
{
    private readonly ILogger<UpdateAreaAfectadaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IGeometryValidator _geometryValidator;

    public UpdateAreaAfectadaCommandHandler(
        ILogger<UpdateAreaAfectadaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IGeometryValidator geometryValidator,
        IMapper mapper
        )
    {
        _logger = logger;
        _geometryValidator = geometryValidator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateAreaAfectadaCommand request, CancellationToken cancellationToken)
    {
        var areaAfectada = await _unitOfWork.Repository<AreaAfectada>().GetByIdAsync(request.Id);

        if (areaAfectada == null)
        {
            _logger.LogWarning($"No se encontro area afectada con id: {request.Id}");
            throw new NotFoundException(nameof(AreaAfectada), request.Id);
        }

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

        _mapper.Map(request, areaAfectada, typeof(UpdateAreaAfectadaCommand), typeof(AreaAfectada));

        _unitOfWork.Repository<AreaAfectada>().UpdateEntity(areaAfectada);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el area afectada con id: {request.Id}");

        return Unit.Value;
    }
}
