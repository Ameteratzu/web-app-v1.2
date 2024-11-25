using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.CoordinacionesPma.Commands.CreateOrUpdateCoordinacionPma;
public class CreateOrUpdateCoordinacionPmaCommandHandler: IRequestHandler<CreateOrUpdateCoordinacionPmaCommand, CreateOrUpdateCoordinacionPmaResponse>
{
    private readonly ILogger<CreateOrUpdateCoordinacionPmaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrUpdateCoordinacionPmaCommandHandler(
        ILogger<CreateOrUpdateCoordinacionPmaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOrUpdateCoordinacionPmaResponse> Handle(CreateOrUpdateCoordinacionPmaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateCoordinacionPmaCommandHandler)} - BEGIN");

        DireccionCoordinacionEmergencia direccionCoordinacionEmergencia;

        // Si el IdDireccionCoordinacionEmergencia es proporcionado, intentar actualizar, si no, crear nueva instancia
        if (request.IdDireccionCoordinacionEmergencia.HasValue)
        {
            var spec = new DireccionCoordinacionEmergenciaWithCoordinacionPma(request.IdDireccionCoordinacionEmergencia.Value);
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

        // Validar los IdProvincia y IdMunicipio de las CoordinacionesPMA en el listado
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
            var idsMunicipiosInvalidas = idsMunicipios.Except(idsMunicipiosExistentes).ToList();

            if (idsMunicipiosInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Municipio: {string.Join(", ", idsMunicipiosInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(Municipio), string.Join(", ", idsMunicipiosInvalidas));
            }
        }

        // Mapear y actualizar/crear las coordinaciones PMA de la emergencia
        foreach (var coordinacionPmaDto in request.Coordinaciones)
        {
            if (coordinacionPmaDto.Id.HasValue && coordinacionPmaDto.Id > 0)
            {
                var coordinacion = direccionCoordinacionEmergencia.CoordinacionesPMA.FirstOrDefault(c => c.Id == coordinacionPmaDto.Id.Value);
                if (coordinacion != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(coordinacionPmaDto, coordinacion);
                    coordinacion.Borrado = false;
                }
                else
                {
                    // Crear nueva coordinación
                    var nuevaCoordinacion = _mapper.Map<CoordinacionPMA>(coordinacionPmaDto);
                    nuevaCoordinacion.Id = 0;
                    direccionCoordinacionEmergencia.CoordinacionesPMA.Add(nuevaCoordinacion);
                }
            }
            else
            {
                // Crear nueva coordinación
                var nuevaCoordinacion = _mapper.Map<CoordinacionPMA>(coordinacionPmaDto);
                nuevaCoordinacion.Id = 0;
                direccionCoordinacionEmergencia.CoordinacionesPMA.Add(nuevaCoordinacion);
            }
        }

        // Eliminar lógicamente las coordinaciones PMA que no están presentes en el request solo si IdDireccionCoordinacionEmergencia es existente
        if (request.IdDireccionCoordinacionEmergencia.HasValue)
        {
            var idsEnRequest = request.Coordinaciones
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var coordinacionesParaEliminar = direccionCoordinacionEmergencia.CoordinacionesPMA
            .Where(c => c.Id > 0 && !idsEnRequest.Contains(c.Id))
                .ToList();

            foreach (var coordinacion in coordinacionesParaEliminar)
            {
                _unitOfWork.Repository<CoordinacionPMA>().DeleteEntity(coordinacion);
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
            throw new Exception("No se pudo insertar/actualizar la Coordinación PMA");
        }

        _logger.LogInformation($"{nameof(CreateOrUpdateCoordinacionPmaCommandHandler)} - END");
        return new CreateOrUpdateCoordinacionPmaResponse { IdDireccionCoordinacionEmergencia = direccionCoordinacionEmergencia.Id };
    }
}
