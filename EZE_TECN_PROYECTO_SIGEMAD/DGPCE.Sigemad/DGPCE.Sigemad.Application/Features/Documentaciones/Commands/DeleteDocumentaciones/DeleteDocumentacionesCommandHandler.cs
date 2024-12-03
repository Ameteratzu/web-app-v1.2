using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;

using DGPCE.Sigemad.Application.Features.Evoluciones.Services;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Vms;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentaciones;

public class DeleteDocumentacionesCommandHandler : IRequestHandler<DeleteDocumentacionesCommand>
{

    private readonly ILogger<DeleteDocumentacionesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEvolucionService _evolucionService;

    public DeleteDocumentacionesCommandHandler(
        ILogger<DeleteDocumentacionesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEvolucionService evolucionService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _evolucionService = evolucionService;
    }

    public async Task<Unit> Handle(DeleteDocumentacionesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(DeleteDocumentacionesCommandHandler)} - END");

        var documentacion = await _unitOfWork.Repository<Documentacion>()
            .GetByIdWithSpec(new DetalleDocumentacionById(request.Id));

        if (documentacion is null)
        {
            _logger.LogWarning($"No se encontró documentación con id: {request.Id}");
            throw new NotFoundException(nameof(Documentacion), request.Id);
        }

        // Verificar si es el último registro por fecha de creación
        var ultimoRegistro = await _unitOfWork.Repository<Documentacion>()
            .GetAsync(d => d.FechaCreacion > documentacion.FechaCreacion && d.Id != documentacion.Id  && !d.Borrado);

        if (ultimoRegistro.Any())
        {
            // No es el último registro
            _logger.LogWarning($"El registro: {request.Id} no es el último");
            throw new LastRegistrationException(nameof(Documentacion), request.Id);
        }
        

        _unitOfWork.Repository<Documentacion>().DeleteEntity(documentacion);
        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo eliminar la documentación");
        }

        _logger.LogInformation($"{nameof(DeleteDocumentacionesCommandHandler)} - END");
        return Unit.Value;
    }
}
