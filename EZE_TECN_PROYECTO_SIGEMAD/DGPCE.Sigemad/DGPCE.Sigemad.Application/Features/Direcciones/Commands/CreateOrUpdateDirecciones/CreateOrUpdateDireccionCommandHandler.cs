using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
internal class CreateOrUpdateDireccionCommandHandler : IRequestHandler<CreateOrUpdateDireccionCommand, CreateOrUpdateDireccionResponse>
{
    private readonly ILogger<CreateImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrUpdateDireccionCommandHandler(
        ILogger<CreateImpactoEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOrUpdateDireccionResponse> Handle(CreateOrUpdateDireccionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - BEGIN");

        DireccionCoordinacionEmergencia direccionCoordinacionEmergencia;

        // Si el IdDireccionCoordinacionEmergencia es proporcionado, intentar actualizar, si no, crear nueva instancia
        if (request.IdDireccionCoordinacionEmergencia.HasValue)
        {
            var spec = new DireccionCoordinacionEmergenciaWithDirecciones(request.IdDireccionCoordinacionEmergencia.Value);
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

        // Validar los IdTipoDireccionEmergencia de las Direcciones en el listado
        var idsTipoDireccion = request.Direcciones.Select(d => d.IdTipoDireccionEmergencia).Distinct();
        var tiposDireccionExistentes = await _unitOfWork.Repository<TipoDireccionEmergencia>().GetAsync(td => idsTipoDireccion.Contains(td.Id));

        if (tiposDireccionExistentes.Count() != idsTipoDireccion.Count())
        {
            var idsTipoDireccionExistentes = tiposDireccionExistentes.Select(td => td.Id).ToList();
            var idsTipoDireccionInvalidos = idsTipoDireccion.Except(idsTipoDireccionExistentes).ToList();

            if (idsTipoDireccionInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de TipoDireccionEmergencia: {string.Join(", ", idsTipoDireccionInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(TipoDireccionEmergencia), string.Join(", ", idsTipoDireccionInvalidos));
            }
        }

        // Mapear y actualizar/crear las direcciones de la emergencia
        foreach (var direccionDto in request.Direcciones)
        {
            if (direccionDto.Id.HasValue && direccionDto.Id > 0)
            {
                var direccion = direccionCoordinacionEmergencia.Direcciones.FirstOrDefault(d => d.Id == direccionDto.Id.Value);
                if (direccion != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(direccionDto, direccion);
                    direccion.Borrado = false;
                }
                else
                {
                    // Crear nueva dirección
                    var nuevaDireccion = _mapper.Map<Direccion>(direccionDto);
                    direccionCoordinacionEmergencia.Direcciones.Add(nuevaDireccion);
                }
            }
            else
            {
                // Crear nueva dirección
                var nuevaDireccion = _mapper.Map<Direccion>(direccionDto);
                direccionCoordinacionEmergencia.Direcciones.Add(nuevaDireccion);
            }
        }

        // Eliminar lógicamente las direcciones que no están presentes en el request solo si IdDireccionCoordinacionEmergencia es existente
        if (request.IdDireccionCoordinacionEmergencia.HasValue)
        {
            // Solo las direcciones con Id existente (no nuevas)
            var idsEnRequest = request.Direcciones
                .Where(d => d.Id.HasValue && d.Id > 0)
                .Select(d => d.Id)
                .ToList();

            var direccionesParaEliminar = direccionCoordinacionEmergencia.Direcciones
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id)) // Solo las direcciones que ya existen en la base de datos y no están en el request
                .ToList();
            foreach (var direccion in direccionesParaEliminar)
            {
                _unitOfWork.Repository<Direccion>().DeleteEntity(direccion);
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
            throw new Exception("No se pudo insertar/actualizar la Dirección y Coordinación de Emergencia");
        }

        _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - END");
        return new CreateOrUpdateDireccionResponse { IdDireccionCoordinacionEmergencia = direccionCoordinacionEmergencia.Id };
    }
}
