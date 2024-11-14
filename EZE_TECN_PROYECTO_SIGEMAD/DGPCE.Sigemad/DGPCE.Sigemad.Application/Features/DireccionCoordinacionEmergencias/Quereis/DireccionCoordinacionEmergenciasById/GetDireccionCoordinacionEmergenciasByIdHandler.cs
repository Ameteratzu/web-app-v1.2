using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Queries.GetIntervencionById;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.DireccionCoordinacionEmergenciasById;
public class GetDireccionCoordinacionEmergenciasByIdHandler : IRequestHandler<GetDireccionCoordinacionEmergenciasById, DireccionCoordinacionEmergenciaVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetIntervencionByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetDireccionCoordinacionEmergenciasByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetIntervencionByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;

    }
    public async Task<DireccionCoordinacionEmergenciaVm> Handle(GetDireccionCoordinacionEmergenciasById request, CancellationToken cancellationToken)
    {

        _logger.LogInformation($"{nameof(GetDireccionCoordinacionEmergenciasById)} - BEGIN");

        var direccionCoordinacionEmergenciaSpec = new DireccionCoordinacionEmergenciaActiveByIdSpecification(new DireccionCoordinacionEmergenciaSpecificationParams { Id = request.Id});
        var direccionCoordinacionEmergencia = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetByIdWithSpec(direccionCoordinacionEmergenciaSpec);
        if (direccionCoordinacionEmergencia == null)
        {
            _logger.LogWarning($"No se encontro direccionCoordinacionEmergencia con id: {request.Id}");
            throw new NotFoundException(nameof(DireccionCoordinacionEmergencia), request.Id);
        }

        _logger.LogInformation($"{nameof(GetDireccionCoordinacionEmergenciasById)} - END");

        var direccionCoordinacionEmergenciaVm = _mapper.Map<DireccionCoordinacionEmergencia, DireccionCoordinacionEmergenciaVm>(direccionCoordinacionEmergencia);
        return direccionCoordinacionEmergenciaVm;

    }
}
