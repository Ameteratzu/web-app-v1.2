using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.CoordinacionCecopis.Commands.CreateCoordinacionCecopi;
public class CreateOrUpdateCoordinacionCecopiCommandHandler : IRequestHandler<CreateOrUpdateCoordinacionCecopiCommand, CreateOrUpdateCoordinacionCecopiResponse>
{
    private readonly ILogger<CreateOrUpdateCoordinacionCecopiCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrUpdateCoordinacionCecopiCommandHandler(
        ILogger<CreateOrUpdateCoordinacionCecopiCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOrUpdateCoordinacionCecopiResponse> Handle(CreateOrUpdateCoordinacionCecopiCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateCoordinacionCecopiCommandHandler)} - BEGIN");

        DireccionCoordinacionEmergencia direccionCoordinacionEmergencia;

        // Si el IdDireccionCoordinacionEmergencia es proporcionado, intentar actualizar, si no, crear nueva instancia
        if (request.IdDireccionCoordinacionEmergencia.HasValue)
        {
            var spec = new DireccionCoordinacionEmergenciaWithCoordinacionCecopis(request.IdDireccionCoordinacionEmergencia.Value);
            direccionCoordinacionEmergencia = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetByIdWithSpec(spec);
            if (direccionCoordinacionEmergencia is null || direccionCoordinacionEmergencia.Borrado)
            {
                _logger.LogWarning($"request.IdDireccionCoordinacionEmergencia: {request.IdDireccionCoordinacionEmergencia}, no encontrado");
                throw new NotFoundException(nameof(DireccionCoordinacionEmergencia), request.IdDireccionCoordinacionEmergencia);
            }
        }
        else
        {
            // Validar si el IdIncendio es válido
            var incendio = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.IdIncendio);
            if (incendio is null || incendio.Borrado)
            {
                _logger.LogWarning($"request.IdIncendio: {request.IdIncendio}, no encontrado");
                throw new NotFoundException(nameof(Incendio), request.IdIncendio);
            }

            // Crear nueva Dirección y Coordinación de Emergencia
            direccionCoordinacionEmergencia = new DireccionCoordinacionEmergencia
            {
                IdIncendio = request.IdIncendio
            };
        }

        // Validar los IdProvincia e IdMunicipio de las Coordinaciones en el listado
        var idsProvincias = request.Coordinaciones.Select(c => c.IdProvincia).Distinct();
        var provinciasExistentes = await _unitOfWork.Repository<Provincia>().GetAsync(p => idsProvincias.Contains(p.Id));

        if (provinciasExistentes.Count() != idsProvincias.Count())
        {
            var idsProvinciasExistentes = provinciasExistentes.Select(p => p.Id).ToList();
            var idsProvinciasInvalidas = idsProvincias.Except(idsProvinciasExistentes).ToList();

            if (idsProvinciasInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Provincia: {string.Join(", ", idsProvinciasInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(Provincia), string.Join(", ", idsProvinciasInvalidas));
            }
        }

        var idsMunicipios = request.Coordinaciones.Select(c => c.IdMunicipio).Distinct();
        var municipiosExistentes = await _unitOfWork.Repository<Municipio>().GetAsync(m => idsMunicipios.Contains(m.Id));

        if (municipiosExistentes.Count() != idsMunicipios.Count())
        {
            var idsMunicipiosExistentes = municipiosExistentes.Select(m => m.Id).ToList();
            var idsMunicipiosInvalidos = idsMunicipios.Except(idsMunicipiosExistentes).ToList();

            if (idsMunicipiosInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Municipio: {string.Join(", ", idsMunicipiosInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(Municipio), string.Join(", ", idsMunicipiosInvalidos));
            }
        }

        // Mapear y actualizar/crear las coordinaciones del CECOPI
        foreach (var coordinacionDto in request.Coordinaciones)
        {
            if (coordinacionDto.Id.HasValue && coordinacionDto.Id > 0)
            {
                var coordinacion = direccionCoordinacionEmergencia.CoordinacionesCecopi.FirstOrDefault(c => c.Id == coordinacionDto.Id.Value);
                if (coordinacion != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(coordinacionDto, coordinacion);
                    coordinacion.Borrado = false;
                }
                else
                {
                    // Crear nueva coordinación
                    var nuevaCoordinacion = _mapper.Map<CoordinacionCecopi>(coordinacionDto);
                    nuevaCoordinacion.Id = 0;
                    direccionCoordinacionEmergencia.CoordinacionesCecopi.Add(nuevaCoordinacion);
                }
            }
            else
            {
                // Crear nueva coordinación
                var nuevaCoordinacion = _mapper.Map<CoordinacionCecopi>(coordinacionDto);
                nuevaCoordinacion.Id = 0;
                direccionCoordinacionEmergencia.CoordinacionesCecopi.Add(nuevaCoordinacion);
            }
        }

        // Eliminar lógicamente las coordinaciones que no están presentes en el request solo si IdDireccionCoordinacionEmergencia es existente
        if (request.IdDireccionCoordinacionEmergencia.HasValue)
        {
            // Solo las coordinaciones con Id existente (no nuevas)
            var idsEnRequest = request.Coordinaciones
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var coordinacionesParaEliminar = direccionCoordinacionEmergencia.CoordinacionesCecopi
                .Where(c => c.Id > 0 && !idsEnRequest.Contains(c.Id)) // Solo las coordinaciones que ya existen en la base de datos y no están en el request
                .ToList();
            foreach (var coordinacion in coordinacionesParaEliminar)
            {
                _unitOfWork.Repository<CoordinacionCecopi>().DeleteEntity(coordinacion);
            }
        }

        if (request.IdDireccionCoordinacionEmergencia.HasValue)
        {
            _unitOfWork.Repository<DireccionCoordinacionEmergencia>().UpdateEntity(direccionCoordinacionEmergencia);
        }
        else
        {
            _unitOfWork.Repository<DireccionCoordinacionEmergencia>().AddEntity(direccionCoordinacionEmergencia);
        }

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar la Coordinación CECOPI");
        }

        _logger.LogInformation($"{nameof(CreateOrUpdateCoordinacionCecopiCommandHandler)} - END");
        return new CreateOrUpdateCoordinacionCecopiResponse { IdDireccionCoordinacionEmergencia = direccionCoordinacionEmergencia.Id };
    }
}
