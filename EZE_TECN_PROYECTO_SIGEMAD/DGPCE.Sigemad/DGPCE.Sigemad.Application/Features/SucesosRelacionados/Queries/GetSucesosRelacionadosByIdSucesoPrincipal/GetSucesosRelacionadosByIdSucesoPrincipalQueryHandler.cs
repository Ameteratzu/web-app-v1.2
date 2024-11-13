using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesosRelacionadosByIdSucesoPrincipal;
public class GetSucesosRelacionadosByIdSucesoPrincipalQueryHandler : IRequestHandler<GetSucesosRelacionadosByIdSucesoPrincipalQuery, List<SucesoRelacionadoVm>>
{
    private readonly ILogger<GetSucesosRelacionadosByIdSucesoPrincipalQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSucesosRelacionadosByIdSucesoPrincipalQueryHandler(
        ILogger<GetSucesosRelacionadosByIdSucesoPrincipalQueryHandler> logger, 
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<SucesoRelacionadoVm>> Handle(GetSucesosRelacionadosByIdSucesoPrincipalQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetSucesosRelacionadosByIdSucesoPrincipalQueryHandler)} - BEGIN");

        var spec = new SucesosRelacionadosActiveByIdSucesoPrincipalSpecification(request.IdSucesoPrincipal);
        var sucesosRelacionados = await _unitOfWork.Repository<SucesoRelacionado>().GetAllWithSpec(spec);

        if (sucesosRelacionados == null)
        {
            _logger.LogWarning($"{nameof(GetSucesosRelacionadosByIdSucesoPrincipalQueryHandler)} - NOT FOUND");
            throw new NotFoundException(nameof(SucesoRelacionado), request.IdSucesoPrincipal);
        }

        var sucesoRelacionadoVm = _mapper.Map<List<SucesoRelacionadoVm>>(sucesosRelacionados);

        _logger.LogInformation($"{nameof(GetSucesosRelacionadosByIdSucesoPrincipalQueryHandler)} - END");
        return sucesoRelacionadoVm;
    }
}
