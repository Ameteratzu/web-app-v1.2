using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Create;
using DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Update;
public class UpdateDireccionCoordinacionEmergenciaCommandHandler : IRequestHandler<UpdateDireccionCoordinacionEmergenciaCommand>
{
    private readonly ILogger<UpdateIncendioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeometryValidator _geometryValidator;
    private readonly ICoordinateTransformationService _coordinateTransformationService;
    private readonly IMapper _mapper;

    public UpdateDireccionCoordinacionEmergenciaCommandHandler(
        ILogger<UpdateIncendioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IGeometryValidator geometryValidator,
        ICoordinateTransformationService coordinateTransformationService,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _geometryValidator = geometryValidator;
        _coordinateTransformationService = coordinateTransformationService;
        _mapper = mapper;
    }


    public async Task<Unit> Handle(UpdateDireccionCoordinacionEmergenciaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateDireccionCoordinacionEmergenciaCommand) + " - BEGIN");

        var incendio = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.IdIncendio);
        if (incendio is null)
        {
            _logger.LogWarning($"request.IdIncendio: {request.IdIncendio}, no encontrado");
            throw new NotFoundException(nameof(Incendio), request.IdIncendio);
        }

        var tipoDireccionEmergencia = await _unitOfWork.Repository<TipoDireccionEmergencia>().GetByIdAsync((int)request.IdTipoDireccionEmergencia);
        if (tipoDireccionEmergencia is null)
        {
            _logger.LogWarning($"request.IdTipoDireccionEmergencia: {request.IdTipoDireccionEmergencia}, no encontrado");
            throw new NotFoundException(nameof(Provincia), request.IdTipoDireccionEmergencia);
        }

        var idProvinciaPMA = await _unitOfWork.Repository<Provincia>().GetByIdAsync(request.IdProvinciaPMA);
        if (idProvinciaPMA is null)
        {
            _logger.LogWarning($"request.IdProvinciaPMA: {request.IdProvinciaPMA}, no encontrado");
            throw new NotFoundException(nameof(Provincia), request.IdProvinciaPMA);
        }

        var idProvinciaCECOPI = await _unitOfWork.Repository<Provincia>().GetByIdAsync(request.IdProvinciaCECOPI);
        if (idProvinciaCECOPI is null)
        {
            _logger.LogWarning($"request.IdProvinciaCECOPI: {request.IdProvinciaCECOPI}, no encontrado");
            throw new NotFoundException(nameof(Provincia), request.IdProvinciaCECOPI);
        }

        var municipioPMA = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipioPMA);
        if (municipioPMA is null)
        {
            _logger.LogWarning($"request.IdMunicipioPMA: {request.IdMunicipioPMA}, no encontrado");
            throw new NotFoundException(nameof(Municipio), request.IdMunicipioPMA);
        }

        var municipioCECOPI = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipioCECOPI);
        if (municipioCECOPI is null)
        {
            _logger.LogWarning($"request.IdMunicipioCECOPI: {request.IdMunicipioCECOPI}, no encontrado");
            throw new NotFoundException(nameof(Municipio), request.IdMunicipioCECOPI);
        }


        if (request.GeoPosicionCECOPI != null)
        {
            if (!_geometryValidator.IsGeometryValidAndInEPSG4326(request.GeoPosicionCECOPI))
            {
                ValidationFailure validationFailure = new ValidationFailure();
                validationFailure.ErrorMessage = "No es una geometria valida o no tiene el EPS4326";

                _logger.LogWarning($"{validationFailure}, geometria -> {request.GeoPosicionCECOPI}");
                throw new ValidationException(new List<ValidationFailure> { validationFailure });
            }
        }

        if (request.GeoPosicionPMA != null)
        {
            if (!_geometryValidator.IsGeometryValidAndInEPSG4326(request.GeoPosicionPMA))
            {
                ValidationFailure validationFailure = new ValidationFailure();
                validationFailure.ErrorMessage = "No es una geometria valida o no tiene el EPS4326";

                _logger.LogWarning($"{validationFailure}, geometria -> {request.GeoPosicionPMA}");
                throw new ValidationException(new List<ValidationFailure> { validationFailure });
            }
        }

        var direccionCoordinacionEmergenciaSpec = new DireccionCoordinacionEmergenciaActiveByIdSpecification(new DireccionCoordinacionEmergenciaSpecificationParams {Id = request.Id });
        var direccionCoordinacionEmergenciaToUpdate = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetByIdWithSpec(direccionCoordinacionEmergenciaSpec);

        if (direccionCoordinacionEmergenciaToUpdate == null)
        {
            _logger.LogWarning($"No se encontro DireccionCoordinacionEmergencia con id: {request.Id}");
            throw new NotFoundException(nameof(DireccionCoordinacionEmergencia), request.Id);
        }

        _mapper.Map(request, direccionCoordinacionEmergenciaToUpdate, typeof(UpdateDireccionCoordinacionEmergenciaCommand), typeof(DireccionCoordinacionEmergencia));

        _unitOfWork.Repository<DireccionCoordinacionEmergencia>().UpdateEntity(direccionCoordinacionEmergenciaToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente la DireccionCoordinacionEmergencia con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateDireccionCoordinacionEmergenciaCommandHandler) + " - END");

        return Unit.Value;

    }



}
