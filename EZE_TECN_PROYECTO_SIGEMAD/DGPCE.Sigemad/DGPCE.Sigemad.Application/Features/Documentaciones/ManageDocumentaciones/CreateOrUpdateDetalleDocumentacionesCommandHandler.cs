using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.DetallesDocumentacion;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;



namespace DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;
public class CreateOrUpdateDetalleDocumentacionesCommandHandler : IRequestHandler<ManageDocumentacionesCommand, CreateOrUpdateDocumentacionResponse>
{
    private readonly ILogger<CreateOrUpdateDetalleDocumentacionesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrUpdateDetalleDocumentacionesCommandHandler(
        ILogger<CreateOrUpdateDetalleDocumentacionesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOrUpdateDocumentacionResponse> Handle(ManageDocumentacionesCommand request, CancellationToken cancellationToken)
    {

        _logger.LogInformation($"{nameof(CreateOrUpdateDetalleDocumentacionesCommandHandler)} - BEGIN");

        Documentacion documentacion;


        // Si el IdDireccionCoordinacionEmergencia es proporcionado, intentar actualizar, si no, crear nueva instancia
        if (request.IdDocumento.HasValue)
        {
            var spec = new DocumentacionSpecification(request.IdDocumento.Value);
            documentacion = await _unitOfWork.Repository<Documentacion>().GetByIdWithSpec(spec);
            if (documentacion is null || documentacion.Borrado)
            {
                _logger.LogWarning($"request.IdDocumento: {request.IdDocumento}, no encontrado");
                throw new NotFoundException(nameof(Documentacion), request.IdDocumento);
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
            documentacion = new Documentacion
            {
                IdIncendio = request.IdIncendio
            };
        }


        // Validar los IdTipoDocumento y IdProcedenciaDestionos de los detallesCoordinacion en el listado
        var idsTipoDocumento = request.DetallesDocumentaciones.Select(c => c.IdTipoDocumento).Distinct();
        var tipoDocumentoExistentes = await _unitOfWork.Repository<TipoDocumento>().GetAsync(p => idsTipoDocumento.Contains(p.Id));

        if (tipoDocumentoExistentes.Count() != idsTipoDocumento.Count())
        {
            var idsTipoDocumentosExistentes = tipoDocumentoExistentes.Select(p => p.Id).ToList();
            var idsTipoDocumentosInvalidas = idsTipoDocumento.Except(idsTipoDocumentosExistentes).ToList();

            if (idsTipoDocumentosInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Tipo Documento: {string.Join(", ", idsTipoDocumentosInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(TipoDocumento), string.Join(", ", idsTipoDocumentosInvalidas));
            }
        }

        var idsDocumentacionProcedenciaDestinos = request.DetallesDocumentaciones.SelectMany(d => d.DocumentacionProcedenciasDestinos).Distinct();
        var documentacionProcedenciaDestinosExistentes = await _unitOfWork.Repository<ProcedenciaDestino>().GetAsync(ic => idsDocumentacionProcedenciaDestinos.Contains(ic.Id));

        if (documentacionProcedenciaDestinosExistentes.Count() != idsDocumentacionProcedenciaDestinos.Count())
        {
            var idsDocumentacionProcedenciaDestinosExistentes = documentacionProcedenciaDestinosExistentes.Select(ic => ic.Id).ToList();
            var idsDocumentacionProcedenciaDestinosInvalidos = idsDocumentacionProcedenciaDestinos.Except(idsDocumentacionProcedenciaDestinosExistentes).ToList();

            if (idsDocumentacionProcedenciaDestinosInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de procedencia destino: {string.Join(", ", idsDocumentacionProcedenciaDestinosInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(ProcedenciaDestino), string.Join(", ", idsDocumentacionProcedenciaDestinosInvalidos));
            }

        }


        // Mapear y actualizar/crear los detalles del documento
        foreach (var detalleDocumentoDto in request.DetallesDocumentaciones)
        {
            if (detalleDocumentoDto.Id.HasValue && detalleDocumentoDto.Id > 0)
            {
                var detalleDocumentacion = documentacion.DetalleDocumentaciones.FirstOrDefault(c => c.Id == detalleDocumentoDto.Id.Value);

                if (detalleDocumentacion != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(detalleDocumentoDto, detalleDocumentacion);
                    detalleDocumentacion.Borrado = false;
                }
                else
                {
                    // Crear nuevo detalle de documentacion
                    var nuevoDetalleDocumentacion = _mapper.Map<DetalleDocumentacion>(detalleDocumentoDto);
                    nuevoDetalleDocumentacion.Id = 0;
                    documentacion.DetalleDocumentaciones.Add(nuevoDetalleDocumentacion);
                }
            }
            else
            {
                // Crear nuevo detalle de documentacionionn
                var nuevoDetalleDocumentacion = _mapper.Map<DetalleDocumentacion>(detalleDocumentoDto);
                nuevoDetalleDocumentacion.Id = 0;
                documentacion.DetalleDocumentaciones.Add(nuevoDetalleDocumentacion);
            }
        }


        // Eliminar lógicamente las coordinaciones que no están presentes en el request solo si IdDireccionCoordinacionEmergencia es existente
        if (request.IdDocumento.HasValue)
        {
            // Solo las coordinaciones con Id existente (no nuevas)
            var idsEnRequest = request.DetallesDocumentaciones
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var detallesDocumentacionParaEliminar = documentacion.DetalleDocumentaciones
                .Where(c => c.Id > 0 && !idsEnRequest.Contains(c.Id)) // Solo las coordinaciones que ya existen en la base de datos y no están en el request
                .ToList();
            foreach (var coordinacion in detallesDocumentacionParaEliminar)
            {
                _unitOfWork.Repository<DetalleDocumentacion>().DeleteEntity(coordinacion);
            }
        }



        if (request.IdDocumento.HasValue)
        {
            _unitOfWork.Repository<Documentacion>().UpdateEntity(documentacion);
        }
        else
        {
            _unitOfWork.Repository<Documentacion>().AddEntity(documentacion);
        }

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar los detalles de la documentacion");
        }

        _logger.LogInformation($"{nameof(CreateOrUpdateDetalleDocumentacionesCommandHandler)} - END");
        return new CreateOrUpdateDocumentacionResponse { IdDocumentacion = documentacion.Id };

    }



    private async Task UpdateProcedenciaDestinoAsync(DetalleDocumentacion detalle, List<int> procedenciasDestinos)
    {
        // 1. Asegúrate de que las relaciones actuales están cargadas
        await _unitOfWork.Repository<DetalleDocumentacion>()
            .GetByIdWithSpec(new DetalleDocumentacionSpecification(detalle.Id));

        // 2. Obtener los IDs actuales en la base de datos
        var procedenciasActuales = detalle.DocumentacionProcedenciaDestinos
            .Select(p => p.IdProcedenciaDestino)
            .ToList();


        // 3. Identificar relaciones a eliminar
        var idsAEliminar = procedenciasActuales.Except(procedenciasDestinos).ToList();
        var procedenciasAEliminar = detalle.DocumentacionProcedenciaDestinos
            .Where(p => idsAEliminar.Contains(p.IdProcedenciaDestino))
            .ToList();

        foreach (var procedencia in procedenciasAEliminar)
        {
            detalle.DocumentacionProcedenciaDestinos.Remove(procedencia);
        }

        // 3. Identifica relaciones a agregar o reactivar
        foreach (var idProcedenciaDestino in procedenciasDestinos)
        {
            var procedenciaExistente = detalle.DocumentacionProcedenciaDestinos
                .FirstOrDefault(p => p.IdProcedenciaDestino == idProcedenciaDestino);

            if (procedenciaExistente == null) // Nueva relación
            {
                detalle.DocumentacionProcedenciaDestinos.Add(new DocumentacionProcedenciaDestino
                {
                    IdDetalleDocumentacion = detalle.Id,
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
