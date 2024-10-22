using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands.CreateIntervencionMedios;
public class CreateIntervencionMedioCommandHandler : IRequestHandler<CreateIntervencionMedioCommand, CreateIntervencionMedioResponse>
{
    private readonly ILogger<CreateIntervencionMedioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateIntervencionMedioCommandHandler(
        ILogger<CreateIntervencionMedioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateIntervencionMedioResponse> Handle(CreateIntervencionMedioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateIntervencionMedioCommandHandler)} - BEGIN");

        var evolucionSpec = new EvolucionActiveByIdSpecification(request.IdEvolucion);
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);
        if (evolucion == null)
        {
            _logger.LogWarning($"No se encontro evolucion con id: {request.IdEvolucion}");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
        }

        var tipoIntervencion = await _unitOfWork.Repository<TipoIntervencionMedio>().GetByIdAsync(request.IdTipoIntervencionMedio);
        if (tipoIntervencion is null)
        {
            _logger.LogWarning($"request.IdTipoIntervencionMedio: {request.IdTipoIntervencionMedio}, no encontrado");
            throw new NotFoundException(nameof(TipoIntervencionMedio), request.IdTipoIntervencionMedio);
        }

        var caracterMedio = await _unitOfWork.Repository<CaracterMedio>().GetByIdAsync(request.IdCaracterMedio);
        if (caracterMedio is null)
        {
            _logger.LogWarning($"request.IdCaracterMedio: {request.IdCaracterMedio}, no encontrado");
            throw new NotFoundException(nameof(CaracterMedio), request.IdCaracterMedio);
        }

        var clasificacionMedio = await _unitOfWork.Repository<ClasificacionMedio>().GetByIdAsync(request.IdClasificacionMedio);
        if (clasificacionMedio is null)
        {
            _logger.LogWarning($"request.IdClasificacionMedio: {request.IdClasificacionMedio}, no encontrado");
            throw new NotFoundException(nameof(ClasificacionMedio), request.IdClasificacionMedio);
        }

        var titularMedio = await _unitOfWork.Repository<TitularidadMedio>().GetByIdAsync(request.IdTitularidadMedio);
        if (titularMedio is null)
        {
            _logger.LogWarning($"request.IdTitularidadMedio: {request.IdTitularidadMedio}, no encontrado");
            throw new NotFoundException(nameof(TitularidadMedio), request.IdTitularidadMedio);
        }

        var municipio = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipio);
        if (municipio is null)
        {
            _logger.LogWarning($"request.IdMunicipio: {request.IdMunicipio}, no encontrado");
            throw new NotFoundException(nameof(Municipio), request.IdMunicipio);
        }

        var intervencionMedioEntity = _mapper.Map<IntervencionMedio>(request);
        _unitOfWork.Repository<IntervencionMedio>().AddEntity(intervencionMedioEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo intervencion de medio");
        }
        _logger.LogInformation($"La intervencion de medio con id {intervencionMedioEntity.Id} fue creado correctamente");

        _logger.LogInformation($"{nameof(CreateIntervencionMedioCommandHandler)} - END");
        return new CreateIntervencionMedioResponse { Id = intervencionMedioEntity.Id };
    }
}
