using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
public class GetSucesoRelacionadoByIdQueryHandler : IRequestHandler<GetSucesoRelacionadoByIdQuery, SucesoRelacionadoVm>
{
    private readonly ILogger<GetSucesoRelacionadoByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSucesoRelacionadoByIdQueryHandler(
        ILogger<GetSucesoRelacionadoByIdQueryHandler> logger, 
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<SucesoRelacionadoVm> Handle(GetSucesoRelacionadoByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetSucesoRelacionadoByIdQueryHandler)} - BEGIN");

        var spec = new SucesoRelacionadoActiveByIdSpecification(request.Id);
        var sucessRelacionado = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(spec);

        if (sucessRelacionado == null)
        {
            _logger.LogWarning($"{nameof(GetSucesoRelacionadoByIdQueryHandler)} - NOT FOUND");
            throw new NotFoundException(nameof(SucesoRelacionado), request.Id);
        }

        var sucesoRelacionadoVm = _mapper.Map<SucesoRelacionadoVm>(sucessRelacionado);

        _logger.LogInformation($"{nameof(GetSucesoRelacionadoByIdQueryHandler)} - END");
        return sucesoRelacionadoVm;
    }
}
