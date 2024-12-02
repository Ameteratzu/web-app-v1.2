using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.DetalleOtraInformaciones;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.ManageOtraInformaciones;
public class ManageOtraInformacionCommandHandler : IRequestHandler<ManageOtraInformacionCommand, ManageOtraInformacionResponse>
{
    private readonly ILogger<ManageOtraInformacionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageOtraInformacionCommandHandler(
        ILogger<ManageOtraInformacionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageOtraInformacionResponse> Handle(ManageOtraInformacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageOtraInformacionCommandHandler)} - BEGIN");

        OtraInformacion otraInformacion;

        // Verificar si se está actualizando un registro existente
        if (request.IdOtraInformacion.HasValue && request.IdOtraInformacion.Value > 0)
        {
            var spec = new OtraInformacionWithDetailsAndProcedenciasSpecification(request.IdOtraInformacion.Value);
            otraInformacion = await _unitOfWork.Repository<OtraInformacion>().GetByIdWithSpec(spec);
            if (otraInformacion is null || otraInformacion.Borrado)
            {
                _logger.LogWarning($"IdOtraInformacion: {request.IdOtraInformacion}, no encontrado");
                throw new NotFoundException(nameof(OtraInformacion), request.IdOtraInformacion);
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

            // Crear nueva instancia de OtraInformacion
            otraInformacion = new OtraInformacion
            {
                IdIncendio = request.IdIncendio
            };
        }

        // Validar los Id de Medio y Procedencia/Destino
        var idsMedios = request.Lista.Select(c => c.IdMedio).Distinct();
        var mediosExistentes = await _unitOfWork.Repository<Medio>().GetAsync(p => idsMedios.Contains(p.Id));

        if (mediosExistentes.Count() != idsMedios.Count())
        {
            var idsMediosExistentes = mediosExistentes.Select(p => p.Id).ToList();
            var idsMediosInvalidas = idsMedios.Except(idsMediosExistentes).ToList();

            if (idsMediosInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Medio: {string.Join(", ", idsMediosInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(Medio), string.Join(", ", idsMediosInvalidas));
            }
        }

        // Obtener todos los Ids de Procedencia/Destino desde la lista anidada dentro de los detalles
        var idsProcedenciaDestino = request.Lista
            .Where(c => c.IdsProcedenciasDestinos != null) // Asegurarse de que la lista no sea nula
            .SelectMany(c => c.IdsProcedenciasDestinos)
            .Distinct();

        var procedenciasExistentes = await _unitOfWork.Repository<ProcedenciaDestino>().GetAsync(p => idsProcedenciaDestino.Contains(p.Id));

        // Validar los Ids de Procedencia/Destino
        if (procedenciasExistentes.Count() != idsProcedenciaDestino.Count())
        {
            var idsProcedenciasExistentes = procedenciasExistentes.Select(p => p.Id).ToList();
            var idsProcedenciasInvalidas = idsProcedenciaDestino.Except(idsProcedenciasExistentes).ToList();

            if (idsProcedenciasInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia/Destino: {string.Join(", ", idsProcedenciasInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(ProcedenciaDestino), string.Join(", ", idsProcedenciasInvalidas));
            }
        }

        // Procesar los Detalles de OtraInformacion: Crear - Actualizar
        foreach (var detalleDto in request.Lista)
        {
            if (detalleDto.Id.HasValue && detalleDto.Id > 0) // Actualización o eliminación lógica
            {
                var detalleExistente = otraInformacion.DetallesOtraInformacion.FirstOrDefault(o => o.Id == detalleDto.Id.Value);
                if (detalleExistente != null)
                {
                    _mapper.Map(detalleDto, detalleExistente);
                    detalleExistente.Borrado = false;
                    await UpdateProcedenciaDestinoAsync(detalleExistente, detalleDto.IdsProcedenciasDestinos);
                    //detalleExistente.ProcedenciasDestinos.Add
                }
            }
            else // Creación
            {
                var nuevoDetalle = _mapper.Map<DetalleOtraInformacion>(detalleDto);
                nuevoDetalle.IdOtraInformacion = 0;
                otraInformacion.DetallesOtraInformacion.Add(nuevoDetalle);
            }
        }

        // Eliminar lógicamente las direcciones que no están presentes en el request solo si IdDireccionCoordinacionEmergencia es existente
        if (request.IdOtraInformacion.HasValue)
        {
            // Solo las direcciones con Id existente (no nuevas)
            var idsEnRequest = request.Lista
                .Where(d => d.Id.HasValue && d.Id > 0)
                .Select(d => d.Id)
                .ToList();

            var direccionesParaEliminar = otraInformacion.DetallesOtraInformacion
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id)) // Solo las direcciones que ya existen en la base de datos y no están en el request
                .ToList();
            foreach (var direccion in direccionesParaEliminar)
            {
                _unitOfWork.Repository<DetalleOtraInformacion>().DeleteEntity(direccion);
            }
        }

        if (request.IdOtraInformacion.HasValue && request.IdOtraInformacion.Value > 0)
        {
            _unitOfWork.Repository<OtraInformacion>().UpdateEntity(otraInformacion);
        }
        else
        {
            _unitOfWork.Repository<OtraInformacion>().AddEntity(otraInformacion);
        }

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar/eliminar los datos de Otra Información");
        }

        _logger.LogInformation($"{nameof(ManageOtraInformacionCommandHandler)} - END");
        return new ManageOtraInformacionResponse { IdOtraInformacion = otraInformacion.Id };
    }

    private async Task UpdateProcedenciaDestinoAsync(
    DetalleOtraInformacion detalle,
    List<int> procedenciasDestinos)
    {
        // 1. Asegúrate de que las relaciones actuales están cargadas
        await _unitOfWork.Repository<DetalleOtraInformacion>()
            .GetByIdWithSpec(new DetalleOtraInformacionWithProcedenciasSpecification(detalle.Id));

        // 2. Obtener los IDs actuales en la base de datos
        var procedenciasActuales = detalle.ProcedenciasDestinos
            .Select(p => p.IdProcedenciaDestino)
            .ToList();


        // 3. Identificar relaciones a eliminar
        var idsAEliminar = procedenciasActuales.Except(procedenciasDestinos).ToList();
        var procedenciasAEliminar = detalle.ProcedenciasDestinos
            .Where(p => idsAEliminar.Contains(p.IdProcedenciaDestino))
            .ToList();

        foreach (var procedencia in procedenciasAEliminar)
        {
            detalle.ProcedenciasDestinos.Remove(procedencia);
        }

        // 3. Identifica relaciones a agregar o reactivar
        foreach (var idProcedenciaDestino in procedenciasDestinos)
        {
            var procedenciaExistente = detalle.ProcedenciasDestinos
                .FirstOrDefault(p => p.IdProcedenciaDestino == idProcedenciaDestino);

            if (procedenciaExistente == null) // Nueva relación
            {
                detalle.ProcedenciasDestinos.Add(new DetalleOtraInformacion_ProcedenciaDestino
                {
                    IdDetalleOtraInformacion = detalle.Id,
                    IdProcedenciaDestino = idProcedenciaDestino,
                });
            }
            else if (procedenciaExistente.Borrado) // Reactivar relación existente
            {
                procedenciaExistente.Borrado = false;
            }
        }

        // 4. Actualizar el objeto principal (se reflejarán los cambios en las relaciones)
        //_unitOfWork.Repository<DetalleOtraInformacion>().UpdateEntity(detalle);
    }







}
