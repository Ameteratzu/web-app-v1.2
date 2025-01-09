using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasByIdIncendioList;
public class GetDCEByIdSucesoListQueryHandler : IRequestHandler<GetDCEByIdSucesoListQuery, IReadOnlyList<DireccionCoordinacionEmergenciaVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDCEByIdSucesoListQueryHandler> _logger;

    public GetDCEByIdSucesoListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,ILogger<GetDCEByIdSucesoListQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
         _logger = logger;

    }
    public async Task<IReadOnlyList<DireccionCoordinacionEmergenciaVm>> Handle(GetDCEByIdSucesoListQuery request, CancellationToken cancellationToken)
    {
    
        _logger.LogInformation($"{nameof(GetDCEByIdSucesoListQueryHandler)} - BEGIN");

        var direccionCoordinacionEmergenciaSpec = new DireccionCoordinacionEmergenciaActiveByIdSpecification(new DireccionCoordinacionEmergenciaSpecificationParams { IdSuceso = request.IdSuceso });
        var direccionCoordinacionEmergencias = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetAllWithSpec(direccionCoordinacionEmergenciaSpec);
        if (direccionCoordinacionEmergencias == null)
        {
            _logger.LogWarning($"No se encontro direccionCoordinacionEmergencias con id de suceso: {request.IdSuceso}");
            throw new NotFoundException(nameof(DireccionCoordinacionEmergencia), request.IdSuceso);
        }

        _logger.LogInformation($"{nameof(GetDCEByIdSucesoListQueryHandler)} - END");

        var direccionCoordinacionEmergenciasVm = _mapper.Map<IReadOnlyList<DireccionCoordinacionEmergencia>, IReadOnlyList<DireccionCoordinacionEmergenciaVm>>(direccionCoordinacionEmergencias);
        return direccionCoordinacionEmergenciasVm;


    }
}
