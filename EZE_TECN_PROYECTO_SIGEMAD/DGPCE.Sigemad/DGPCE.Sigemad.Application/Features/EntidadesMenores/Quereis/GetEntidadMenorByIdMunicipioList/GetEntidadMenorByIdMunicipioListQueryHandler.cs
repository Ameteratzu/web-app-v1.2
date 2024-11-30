using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Application.Specifications.EntidadesMenores;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorByIdMunicipioList;


public class GetEntidadMenorByIdMunicipioListQueryHandler : IRequestHandler<GetEntidadMenorByIdMunicipioListQuery, IReadOnlyList<EntidadMenorVm>>
{
    private readonly ILogger<GetEntidadMenorByIdMunicipioListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEntidadMenorByIdMunicipioListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetEntidadMenorByIdMunicipioListQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IReadOnlyList<EntidadMenorVm>> Handle(GetEntidadMenorByIdMunicipioListQuery request, CancellationToken cancellationToken)
    {

        var municipio = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipio);


        if (municipio == null)
        {
            _logger.LogWarning($"No se encontro municipio con id: {request.IdMunicipio}");
            throw new NotFoundException(nameof(Municipio), request.IdMunicipio);
        }


        var spec = new EntidadesMenoresActiveByIdMunicipioSpecification(request.IdMunicipio);
        var entidadesMenores = await _unitOfWork.Repository<EntidadMenor>()
        .GetAllWithSpec(spec);

        var entidadesmenoresVm = _mapper.Map<IReadOnlyList<EntidadMenor>, IReadOnlyList<EntidadMenorVm>>(entidadesMenores);
        return entidadesmenoresVm;

    }
}
