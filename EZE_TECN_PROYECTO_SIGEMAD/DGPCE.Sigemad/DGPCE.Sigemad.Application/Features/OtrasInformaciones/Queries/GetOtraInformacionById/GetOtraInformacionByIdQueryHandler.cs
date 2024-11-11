using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesList;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Vms;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesById;
public class GetOtraInformacionByIdQueryHandler : IRequestHandler<GetOtraInformacionByIdQuery, List<OtraInformacionVm>>
{
    private readonly ILogger<GetOtraInformacionByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOtraInformacionByIdQueryHandler(
        IUnitOfWork unitOfWork, 
        ILogger<GetOtraInformacionByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<OtraInformacionVm>> Handle(GetOtraInformacionByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - BEGIN");

        var spec = new OtraInformacionActiveByIdSpecification(request.Id);
        var otraInformacion = await _unitOfWork.Repository<OtraInformacion>().GetByIdWithSpec(spec);

        if (otraInformacion is null)
        {
            _logger.LogWarning($"No se encontró otra información con id: {request.Id}");
            throw new NotFoundException(nameof(OtraInformacion), request.Id);
        }

        var specDetalle = new DetalleOtraInformacionByIdOtraInformacionSpecification(request.Id);
        var detalleOtraInformacion = await _unitOfWork.Repository<DetalleOtraInformacion>().GetAllWithSpec(specDetalle);        

        var otraInformacionVms = detalleOtraInformacion
        .Select(detalle =>
        {
            var vm = _mapper.Map<DetalleOtraInformacion, OtraInformacionVm>(detalle);
            vm.IdOtraInformacion = otraInformacion.Id;
            vm.IdIncendio = otraInformacion.IdIncendio;
            vm.IdsProcedenciaDestino = detalle.ProcedenciasDestinos.Select(pd => pd.IdProcedenciaDestino).ToList();
            return vm;
        }).ToList();

        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - END");
        return otraInformacionVms;
    }

    
}
