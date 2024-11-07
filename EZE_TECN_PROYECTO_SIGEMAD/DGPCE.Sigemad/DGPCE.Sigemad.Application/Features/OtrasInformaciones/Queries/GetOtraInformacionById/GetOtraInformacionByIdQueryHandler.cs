using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesList;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesById;
public class GetOtraInformacionByIdQueryHandler : IRequestHandler<GetOtraInformacionByIdQuery, OtraInformacion>
{
    private readonly ILogger<GetOtraInformacionByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public GetOtraInformacionByIdQueryHandler(
        IUnitOfWork unitOfWork, 
        ILogger<GetOtraInformacionByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<OtraInformacion> Handle(GetOtraInformacionByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - BEGIN");

        var spec = new OtraInformacionActiveByIdSpecification(request.Id);
        var otraInformacion = await _unitOfWork.Repository<OtraInformacion>().GetByIdWithSpec(spec);

        if (otraInformacion is null)
        {
            _logger.LogWarning($"No se encontró otra información con id: {request.Id}");
            throw new NotFoundException(nameof(OtraInformacion), request.Id);
        }

        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - END");
        return otraInformacion;
    }

    
}
