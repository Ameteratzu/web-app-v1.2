using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.ManageDocumentaciones;
public class ManageDocumentacionesCommandHandler : IRequestHandler<ManageDocumentacionesCommandList, Unit>
{
    private readonly ILogger<ManageDocumentacionesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageDocumentacionesCommandHandler(
        ILogger<ManageDocumentacionesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(ManageDocumentacionesCommandList request, CancellationToken cancellationToken)
    {

            var documentosParams = new DocumentoSpecificationParams
            {
                IdIncendio = request.IdIncendio,
            };


        var incendio = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.IdIncendio);
        if (incendio is null || incendio.Borrado)
        {
            _logger.LogWarning($"request.IdIncendio: {request.IdIncendio}, no encontrado");
            throw new NotFoundException(nameof(Incendio), request.IdIncendio);
        }

        var spec = new DocumentoSpecification(documentosParams);
            var documentacionesGuardadas = await _unitOfWork.Repository<Documentacion>()
            .GetAllWithSpec(spec);


            var documentacionParaEliminar = documentacionesGuardadas
                    .Where(d => d.Id > 0 && !request.Documentaciones.Where(r => r.Id.HasValue && r.Id > 0).Select(d => d.Id).Contains(d.Id))
                    .ToList();

            var documentacionParaGuardar = request.Documentaciones
              .Where(x => x.Id == null || (x.Id.Value > 0 && !documentacionParaEliminar.Select(d => d.Id).Contains(x.Id.Value)))
              .ToList();

            foreach (var documentacionEliminar in documentacionParaEliminar)
                {
                    _unitOfWork.Repository<Documentacion>().DeleteEntity(documentacionEliminar);
                }

        foreach (var documentacion in documentacionParaGuardar)
        {
            var tipoDocumento = await _unitOfWork.Repository<TipoDocumento>().GetByIdAsync(documentacion.IdTipoDocumento);
            if (tipoDocumento is null)
            {
                _logger.LogWarning($"request.IdTipoDocumento: {documentacion.IdTipoDocumento}, no encontrado");
                throw new NotFoundException(nameof(TipoDocumento), documentacion.IdTipoDocumento);
            }

            var archivo = await _unitOfWork.Repository<Archivo>().GetByIdAsync(documentacion.IdArchivo);
            if (tipoDocumento is null)
            {
                _logger.LogWarning($"request.IdArchivo: {documentacion.IdTipoDocumento}, no encontrado");
                throw new NotFoundException(nameof(Archivo), documentacion.IdTipoDocumento);
            }


            if (documentacion.DocumentacionProcedenciasDestinos != null)
            {
                var idsDocumentacionProcedenciaDestinos = documentacion.DocumentacionProcedenciasDestinos.Select(d => d).Distinct();
                var documentacionProcedenciaDestinosExistentes = await _unitOfWork.Repository<ProcedenciaDestino>().GetAsync(ic => idsDocumentacionProcedenciaDestinos.Contains(ic.Id));

                if (documentacionProcedenciaDestinosExistentes.Count() != idsDocumentacionProcedenciaDestinos.Count())
                {
                    var idsDocumentacionProcedenciaDestinosExistentes = documentacionProcedenciaDestinosExistentes.Select(ic => ic.Id).ToList();
                    var idsDocumentacionProcedenciaDestinosInvalidos = idsDocumentacionProcedenciaDestinos.Except(idsDocumentacionProcedenciaDestinosExistentes).ToList();

                    if (idsDocumentacionProcedenciaDestinosInvalidos.Any())
                    {
                        _logger.LogWarning($"Los siguientes Id's de documentacion procedencia destino: {string.Join(", ", idsDocumentacionProcedenciaDestinosInvalidos)}, no se encontraron");
                        throw new NotFoundException(nameof(DocumentacionProcedenciaDestino), string.Join(", ", idsDocumentacionProcedenciaDestinosInvalidos));
                    }
                }
            }
    

            if (documentacion.Id.HasValue)
            {
                var documentacionGuardada = documentacionesGuardadas.Where(d => d.Id == documentacion.Id).FirstOrDefault();
                if (documentacionGuardada != null)
                {
                    _mapper.Map(documentacion, documentacionGuardada, typeof(CreateRegistroCommand), typeof(Documentacion));
                    _unitOfWork.Repository<Documentacion>().UpdateEntity(documentacionGuardada);
                }          
            }
            else
            {
                var documentacionEntity = _mapper.Map<Documentacion>(documentacion);
                documentacionEntity.IdIncendio = request.IdIncendio;
                _unitOfWork.Repository<Documentacion>().AddEntity(documentacionEntity);
            }
            
        }

        await _unitOfWork.Complete();
        return Unit.Value;
    }
}
