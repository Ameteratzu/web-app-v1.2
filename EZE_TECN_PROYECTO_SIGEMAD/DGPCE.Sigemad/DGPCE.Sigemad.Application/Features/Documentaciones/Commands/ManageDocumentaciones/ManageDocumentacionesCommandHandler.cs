using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Exceptions;
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

        var documentacion = await GetOrCreateDocumentacionAsync(request);
        await ValidateIdsAsync(request);
        await MapAndManageDetallesDocumentacion(request, documentacion);
        await PersistDocumentacionAsync(request, documentacion);

        _logger.LogInformation($"{nameof(ManageDocumentacionesCommandHandler)} - END");
        return new CreateOrUpdateDocumentacionResponse { IdDocumentacion = documentacion.Id };

    }

    private async Task<Documentacion> GetOrCreateDocumentacionAsync(ManageDocumentacionesCommand request)
    {
        if (request.IdDocumento.HasValue && request.IdDocumento.Value > 0)
        {
            var spec = new DocumentacionSpecification(request.IdDocumento.Value);
            var documentacion = await _unitOfWork.Repository<Documentacion>().GetByIdWithSpec(spec);
            if (documentacion is null || documentacion.Borrado)
            {
                _logger.LogWarning($"request.IdDocumento: {request.IdDocumento}, no encontrado");
                throw new NotFoundException(nameof(Documentacion), request.IdDocumento);
            }
            return documentacion;
        }
        else
        {
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (suceso is null || suceso.Borrado)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            return new Documentacion { IdSuceso = request.IdSuceso };
        }
    }

    private async Task ValidateIdsAsync(ManageDocumentacionesCommand request)
    {
        await ValidateTipoDocumentosAsync(request);
        await ValidateProcedenciasDestinosAsync(request);
    }

    private async Task ValidateTipoDocumentosAsync(ManageDocumentacionesCommand request)
    {
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
    }

    private async Task ValidateProcedenciasDestinosAsync(ManageDocumentacionesCommand request)
    {
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
    }

    private async Task MapAndManageDetallesDocumentacion(ManageDocumentacionesCommand request, Documentacion documentacion)
    {
        foreach (var detalleDocumentoDto in request.DetallesDocumentaciones)
        {
            if (detalleDocumentoDto.Id.HasValue && detalleDocumentoDto.Id > 0)
            {
                var detalleDocumentacion = documentacion.DetallesDocumentacion.FirstOrDefault(c => c.Id == detalleDocumentoDto.Id.Value);

                if (detalleDocumentacion != null)
                {
                    _mapper.Map(detalleDocumentoDto, detalleDocumentacion);
                    detalleDocumentacion.Borrado = false;
                    detalleDocumentacion.Archivo = await MapArchivo(detalleDocumentoDto, detalleDocumentacion.Archivo);
                }
                else
                {
                    var nuevoDetalleDocumentacion = await CreateDetalleDocumentacion(detalleDocumentoDto);
                    documentacion.DetallesDocumentacion.Add(nuevoDetalleDocumentacion);
                }
            }
            else
            {
                var nuevoDetalleDocumentacion = await CreateDetalleDocumentacion(detalleDocumentoDto);
                documentacion.DetallesDocumentacion.Add(nuevoDetalleDocumentacion);
            }
        }

        if (request.IdDocumento.HasValue)
        {
            var idsEnRequest = request.DetallesDocumentaciones
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var detallesDocumentacionParaEliminar = documentacion.DetallesDocumentacion
                .Where(c => c.Id > 0 && c.Borrado == false && !idsEnRequest.Contains(c.Id))
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
    }

    private async Task<Archivo?> MapArchivo(DetalleDocumentacionDto detalleDocumentoDto, Archivo? archivoExistente)
    {
        if (detalleDocumentoDto.Archivo != null)
        {
            var fileEntity = new Archivo
            {
                NombreOriginal = detalleDocumentoDto.Archivo?.FileName ?? string.Empty,
                NombreUnico = $"{Path.GetFileNameWithoutExtension(detalleDocumentoDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{detalleDocumentoDto.Archivo?.Extension ?? string.Empty}",
                Tipo = detalleDocumentoDto.Archivo?.ContentType ?? string.Empty,
                Extension = detalleDocumentoDto.Archivo?.Extension ?? string.Empty,
                PesoEnBytes = detalleDocumentoDto.Archivo?.Length ?? 0,
            };

            fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(detalleDocumentoDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
            fileEntity.FechaCreacion = DateTime.Now;
            return fileEntity;
        }

        return archivoExistente;
    }

    private async Task<DetalleDocumentacion> CreateDetalleDocumentacion(DetalleDocumentacionDto detalleDocumentoDto)
    {
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

        nuevoDetalleDocumentacion.Archivo = await MapArchivo(detalleDocumentoDto, null);

        return nuevoDetalleDocumentacion;
    }

    private async Task PersistDocumentacionAsync(ManageDocumentacionesCommand request, Documentacion documentacion)
    {
        if (request.IdDocumento.HasValue && request.IdDocumento.Value > 0)
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
    }



}
