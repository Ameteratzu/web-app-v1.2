using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
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

        Evolucion evolucion = null;
        if (request.IdEvolucion.HasValue)
        {
            evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdAsync(request.IdEvolucion.Value);
            if (evolucion is null)
            {
                _logger.LogWarning($"idEvolucion: {request.IdEvolucion}, no encontrado");
                throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
            }
        }
        else
        {
            var incendio = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.IdIncendio);
            if (incendio is null || incendio.Borrado)
            {
                _logger.LogWarning($"request.IdIncendio: {request.IdIncendio}, no encontrado");
                throw new NotFoundException(nameof(Incendio), request.IdIncendio);
            }


            evolucion = new Evolucion()
            {
                IdIncendio = request.IdIncendio
            };
        }

        //VALIDACION DE ID EN LISTA DE AREAS AFECTADAS
        var idsProvincias = request.AreasAfectadas.Select(a => a.IdProvincia).Distinct();
        var provinciasExistentes = await _unitOfWork.Repository<Provincia>().GetAsync(p => idsProvincias.Contains(p.Id) && p.Borrado == false);
        if (provinciasExistentes.Count() != idsProvincias.Count())
        {
            var idsProvinciasExistentes = provinciasExistentes.Select(p => p.Id).ToList();
            var idsProvinciasInvalidas = idsProvincias.Except(idsProvinciasExistentes).ToList();

            if (idsProvinciasInvalidas.Any())
            {
                _logger.LogWarning($"Las siguientes Id's de provincias: {string.Join(", ", idsProvinciasInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(Provincia), string.Join(", ", idsProvinciasInvalidas));
            }
        }


        var idsMunicipios = request.AreasAfectadas.Select(a => a.IdMunicipio).Distinct();
        var municipiosExistentes = await _unitOfWork.Repository<Municipio>().GetAsync(p => idsMunicipios.Contains(p.Id) && p.Borrado == false);
        if (municipiosExistentes.Count() != idsMunicipios.Count())
        {
            var idsMunicipiosExistentes = provinciasExistentes.Select(p => p.Id).ToList();
            var idsMunicipiosInvalidas = idsProvincias.Except(idsMunicipiosExistentes).ToList();

            if (idsMunicipiosInvalidas.Any())
            {
                _logger.LogWarning($"Las siguientes Id's de municipios: {string.Join(", ", idsMunicipiosInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(Municipio), string.Join(", ", idsMunicipiosInvalidas));
            }
        }

        var idsEntidadMenor = request.AreasAfectadas
            .Where(a => a.IdEntidadMenor.HasValue)
            .Select(a => a.IdEntidadMenor.Value)
            .Distinct()
            .ToList();

        var entidadMenorExistentes = await _unitOfWork.Repository<EntidadMenor>().GetAsync(p => idsEntidadMenor.Contains(p.Id) && p.Borrado == false);
        if (entidadMenorExistentes.Count() != idsEntidadMenor.Count())
        {
            var idsEntidadMenorExistentes = entidadMenorExistentes.Select(p => p.Id).ToList();
            var idsEntidadMenorInvalidas = idsEntidadMenor.Except(idsEntidadMenorExistentes).ToList();

            if (idsEntidadMenorInvalidas.Any())
            {
                _logger.LogWarning($"Las siguientes Id's de entidad menor: {string.Join(", ", idsEntidadMenorInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(EntidadMenor), string.Join(", ", idsEntidadMenorInvalidas));
            }
        }


        //Validar geometria
        foreach (var item in request.AreasAfectadas)
        {
            if (!_geometryValidator.IsGeometryValidAndInEPSG4326(item.GeoPosicion))
            {
                ValidationFailure validationFailure = new ValidationFailure();
                validationFailure.ErrorMessage = "No es una geometria valida o no tiene el EPS4326";

                _logger.LogWarning($"{validationFailure}, geometria -> {item.GeoPosicion}");
                throw new ValidationException(new List<ValidationFailure> { validationFailure });
            }
        }


        //Guardar en base de datos
        evolucion.AreaAfectadas = _mapper.Map<ICollection<AreaAfectada>>(request.AreasAfectadas);

        if (request.IdEvolucion.HasValue)
        {
            _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);
        }
        else
        {
            _unitOfWork.Repository<Evolucion>().AddEntity(evolucion);
        }

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar area afectada");
        }

        _logger.LogInformation(nameof(CreateAreaAfectadaCommandHandler) + " - END");
        return new CreateAreaAfectadaResponse { IdEvolucion = evolucion.Id };
    }


}
