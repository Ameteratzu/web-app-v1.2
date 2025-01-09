using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.DetallesDocumentacion;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.ManageDocumentaciones;
public class ManageDocumentacionesCommandHandler : IRequestHandler<ManageDocumentacionesCommand, CreateOrUpdateDocumentacionResponse>
{
    private readonly ILogger<ManageDocumentacionesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private const string ARCHIVOS_PATH = "documentacion";

    public ManageDocumentacionesCommandHandler(
        ILogger<ManageDocumentacionesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<CreateOrUpdateDocumentacionResponse> Handle(ManageDocumentacionesCommand request, CancellationToken cancellationToken)
    {

        _logger.LogInformation($"{nameof(ManageDocumentacionesCommandHandler)} - BEGIN");

        Documentacion documentacion;


        // Si el IdDireccionCoordinacionEmergencia es proporcionado, intentar actualizar, si no, crear nueva instancia
        if (request.IdDocumento.HasValue && request.IdDocumento.Value > 0)
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
            // Validar si el IdSuceso es válido
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (suceso is null || suceso.Borrado)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            // Crear nueva Dirección y Coordinación de Emergencia
            documentacion = new Documentacion
            {
                IdSuceso = request.IdSuceso
            };
        }


        /*
        // Validar los IdTipoDocumento y IdProcedenciaDestionos de los detallesCoordinacion en el listado
        var idsArchivos = request.DetallesDocumentaciones.Select(c => c.IdArchivo).Distinct();
        var archivosExistentes = await _unitOfWork.Repository<Archivo>().GetAsync(p => idsArchivos.Contains(p.Id));

        if (archivosExistentes.Count() != idsArchivos.Count())
        {
            var idsArchivosExistentes = archivosExistentes.Select(p => p.Id).ToList();
            var idsArchivosInvalidas = idsArchivos
                 .Where(id => id.HasValue && !idsArchivosExistentes.Contains(id.Value))
                 .ToList();

            if (idsArchivosInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's Archivos: {string.Join(", ", idsArchivosInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(Archivo), string.Join(", ", idsArchivosInvalidas));
            }
        }
        */


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

        var idsDocumentacionProcedenciaDestinos = request.DetallesDocumentaciones
            .SelectMany(d => d.IdsProcedenciasDestinos ?? new List<int>())
            .Distinct();
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
                var detalleDocumentacion = documentacion.DetallesDocumentacion.FirstOrDefault(c => c.Id == detalleDocumentoDto.Id.Value);

                if (detalleDocumentacion != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(detalleDocumentoDto, detalleDocumentacion);
                    detalleDocumentacion.Borrado = false;
                    await UpdateProcedenciaDestinoAsync(detalleDocumentacion, detalleDocumentoDto.IdsProcedenciasDestinos ?? new List<int>());

                }
                else
                {
                    // Crear nuevo detalle de documentacionion
                    var nuevoDetalleDocumentacion = new DetalleDocumentacion
                    {
                        Descripcion = detalleDocumentoDto.Descripcion,
                        DocumentacionProcedenciaDestinos = detalleDocumentoDto.IdsProcedenciasDestinos?
                        .Select(id => new DocumentacionProcedenciaDestino
                        {
                            IdProcedenciaDestino = id
                        }).ToList() ?? new List<DocumentacionProcedenciaDestino>(),
                        FechaHora = detalleDocumentoDto.FechaHora,
                        FechaHoraSolicitud = detalleDocumentoDto.FechaHoraSolicitud,
                        IdTipoDocumento = detalleDocumentoDto.IdTipoDocumento,

                    };

                    // Agregar archivo - BEGIN
                    //var id = Guid.NewGuid();
                    var fileEntity = new Archivo
                    {
                        //Id = id,
                        NombreOriginal = detalleDocumentoDto.Archivo?.FileName ?? string.Empty,
                        //NombreUnico = $"{id}{detalleDocumentoDto.Archivo.Extension}",
                        NombreUnico = $"{Path.GetFileNameWithoutExtension(detalleDocumentoDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{detalleDocumentoDto.Archivo?.Extension ?? string.Empty}",
                        Tipo = detalleDocumentoDto.Archivo?.ContentType ?? string.Empty,
                        Extension = detalleDocumentoDto.Archivo?.Extension ?? string.Empty,
                        PesoEnBytes = detalleDocumentoDto.Archivo?.Length ?? 0,
                    };

                    fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(detalleDocumentoDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
                    fileEntity.FechaCreacion = DateTime.Now;
                    nuevoDetalleDocumentacion.Archivo = fileEntity;
                    // Agregar archivo - END

                    documentacion.DetallesDocumentacion.Add(nuevoDetalleDocumentacion);
                }
            }
            else
            {
                // Crear nuevo detalle de documentacionion
                var nuevoDetalleDocumentacion = new DetalleDocumentacion
                {
                    Descripcion = detalleDocumentoDto.Descripcion,
                    DocumentacionProcedenciaDestinos = detalleDocumentoDto.IdsProcedenciasDestinos?
                    .Select(id => new DocumentacionProcedenciaDestino
                    {
                        IdProcedenciaDestino = id
                    }).ToList() ?? new List<DocumentacionProcedenciaDestino>(),
                    FechaHora = detalleDocumentoDto.FechaHora,
                    FechaHoraSolicitud = detalleDocumentoDto.FechaHoraSolicitud,
                    IdTipoDocumento = detalleDocumentoDto.IdTipoDocumento,

                };

                // Agregar archivo - BEGIN
                //var id = Guid.NewGuid();
                var fileEntity = new Archivo
                {
                    //Id = id,
                    NombreOriginal = detalleDocumentoDto.Archivo?.FileName ?? string.Empty,
                    //NombreUnico = $"{id}{detalleDocumentoDto.Archivo.Extension}",
                    NombreUnico = $"{Path.GetFileNameWithoutExtension(detalleDocumentoDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{detalleDocumentoDto.Archivo?.Extension ?? string.Empty}",
                    Tipo = detalleDocumentoDto.Archivo?.ContentType ?? string.Empty,
                    Extension = detalleDocumentoDto.Archivo?.Extension ?? string.Empty,
                    PesoEnBytes = detalleDocumentoDto.Archivo?.Length ?? 0,
                };

                fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(detalleDocumentoDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
                fileEntity.FechaCreacion = DateTime.Now;
                nuevoDetalleDocumentacion.Archivo = fileEntity;
                // Agregar archivo - END

                documentacion.DetallesDocumentacion.Add(nuevoDetalleDocumentacion);
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

            var detallesDocumentacionParaEliminar = documentacion.DetallesDocumentacion
                .Where(c => c.Id > 0 && c.Borrado == false && !idsEnRequest.Contains(c.Id)) // Solo las coordinaciones que ya existen en la base de datos y no están en el request
                .ToList();
            foreach (var detalleAEliminar in detallesDocumentacionParaEliminar)
            {
                if (detalleAEliminar.IdArchivo.HasValue)
                {
                    _unitOfWork.Repository<Archivo>().DeleteEntity(detalleAEliminar.Archivo);
                }

                _unitOfWork.Repository<DetalleDocumentacion>().DeleteEntity(detalleAEliminar);
            }
        }



        if (request.IdDocumento.HasValue && request.IdDocumento.Value > 0)
        {
            if (request.DetallesDocumentaciones.Any())
            {
                _unitOfWork.Repository<Documentacion>().UpdateEntity(documentacion);
            }
            else
            {
                _unitOfWork.Repository<Documentacion>().DeleteEntity(documentacion);
            }
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

        _logger.LogInformation($"{nameof(ManageDocumentacionesCommandHandler)} - END");
        return new CreateOrUpdateDocumentacionResponse { IdDocumentacion = documentacion.Id };

    }



    private async Task UpdateProcedenciaDestinoAsync(DetalleDocumentacion detalle, List<int> procedenciasDestinos)
    {
        // 1. Asegúrate de que las relaciones actuales están cargadas
        await _unitOfWork.Repository<DetalleDocumentacion>()
           .GetByIdWithSpec(new DetalleDocumentacionSpecification(detalle.Id));

        // 2. Obtener los IDs actuales en la base de datos
        var procedenciasActuales = detalle.DocumentacionProcedenciaDestinos?
            .Select(p => p.IdProcedenciaDestino)
            .ToList() ?? new List<int>();


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

    }



}
