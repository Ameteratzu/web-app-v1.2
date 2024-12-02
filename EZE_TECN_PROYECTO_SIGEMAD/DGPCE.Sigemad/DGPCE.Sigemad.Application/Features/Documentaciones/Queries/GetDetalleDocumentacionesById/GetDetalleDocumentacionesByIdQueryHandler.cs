using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Vms;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDetalleDocumentacionesById;

public class GetDetalleDocumentacionesByIdQueryHandler : IRequestHandler<GetDetalleDocumentacionesByIdQuery, DocumentacionDto>
{
    private readonly ILogger<GetDetalleDocumentacionesByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDetalleDocumentacionesByIdQueryHandler(
        ILogger<GetDetalleDocumentacionesByIdQueryHandler> logger,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DocumentacionDto> Handle(GetDetalleDocumentacionesByIdQuery request, CancellationToken cancellationToken)
    {
        var documentacion = await _unitOfWork.Repository<Documentacion>()
        .GetByIdWithSpec(new DetalleDocumentacionById(request.Id));

        if (documentacion == null)
        {
            _logger.LogWarning($"No se encontro documentación con id: {request.Id}");
            throw new NotFoundException(nameof(Documentacion), request.Id);
        }


        // Filtrar ProcedenciasDestinos que no están borrados
        documentacion.DetallesDocumentacion = documentacion.DetallesDocumentacion
            .Select(detalle =>
            {
                detalle.DocumentacionProcedenciaDestinos = detalle.DocumentacionProcedenciaDestinos
                    .Where(pd => !pd.Borrado)
                    .ToList();
                return detalle;
            })
            .ToList();


        var documentacionDto = _mapper.Map<Documentacion, DocumentacionDto>(documentacion);
        return documentacionDto;
    }
}