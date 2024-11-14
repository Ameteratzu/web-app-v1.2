using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Quereis.GetDireccionCoordinacionEmergenciasByIdIncendioList;
public class GetDCEByIdIncendioListHandler : IRequestHandler<GetDCEByIdIncendioListQuery, IReadOnlyList<DireccionCoordinacionEmergenciaVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDCEByIdIncendioListHandler> _logger;

    public GetDCEByIdIncendioListHandler(IUnitOfWork unitOfWork, IMapper mapper,ILogger<GetDCEByIdIncendioListHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
         _logger = logger;

    }
    public async Task<IReadOnlyList<DireccionCoordinacionEmergenciaVm>> Handle(GetDCEByIdIncendioListQuery request, CancellationToken cancellationToken)
    {
    
        _logger.LogInformation($"{nameof(GetDCEByIdIncendioListHandler)} - BEGIN");

        var direccionCoordinacionEmergenciaSpec = new DireccionCoordinacionEmergenciaActiveByIdSpecification(new DireccionCoordinacionEmergenciaSpecificationParams { IdIncendio = request.IdIncendio });
        var direccionCoordinacionEmergencias = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetAllWithSpec(direccionCoordinacionEmergenciaSpec);
        if (direccionCoordinacionEmergencias == null)
        {
            _logger.LogWarning($"No se encontro direccionCoordinacionEmergencias con id de incendio: {request.IdIncendio}");
            throw new NotFoundException(nameof(DireccionCoordinacionEmergencia), request.IdIncendio);
        }

        _logger.LogInformation($"{nameof(GetDCEByIdIncendioListHandler)} - END");

        var direccionCoordinacionEmergenciasVm = _mapper.Map<IReadOnlyList<DireccionCoordinacionEmergencia>, IReadOnlyList<DireccionCoordinacionEmergenciaVm>>(direccionCoordinacionEmergencias);
        return direccionCoordinacionEmergenciasVm;


    }
}
