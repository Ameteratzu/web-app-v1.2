using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.SucesoRelacionados;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
public class ManageSucesoRelacionadosCommandHandler : IRequestHandler<ManageSucesoRelacionadosCommand, ManageSucesoRelacionadoResponse>
{
    private readonly ILogger<ManageSucesoRelacionadosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageSucesoRelacionadosCommandHandler(
        ILogger<ManageSucesoRelacionadosCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageSucesoRelacionadoResponse> Handle(ManageSucesoRelacionadosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageSucesoRelacionadosCommandHandler)} - BEGIN");

        SucesoRelacionado sucesoRelacionado;

        // Verificar si se está actualizando un registro existente
        if (request.IdSucesoRelacionado.HasValue && request.IdSucesoRelacionado.Value > 0)
        {
            var spec = new SucesoRelacionadoActiveByIdSpecification(request.IdSucesoRelacionado.Value);
            sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(spec);
            if (sucesoRelacionado is null || sucesoRelacionado.Borrado)
            {
                _logger.LogWarning($"IdOtraInformacion: {request.IdSucesoRelacionado}, no encontrado");
                throw new NotFoundException(nameof(SucesoRelacionado), request.IdSucesoRelacionado);
            }
        }
        else
        {
            // Validar si el Suceso es válido
            var incendio = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (incendio is null || incendio.Borrado)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            // Crear nueva instancia de OtraInformacion
            sucesoRelacionado = new SucesoRelacionado
            {
                IdSucesoPrincipal = request.IdSuceso
            };
        }

        // Validar los Id de Sucesos
        var idsSucesosAsociados = request.IdsSucesosAsociados.Distinct();
        var sucesosExistentes = await _unitOfWork.Repository<Suceso>().GetAsync(p => idsSucesosAsociados.Contains(p.Id));

        if (sucesosExistentes.Count() != idsSucesosAsociados.Count())
        {
            var idsSucesosExistentes = sucesosExistentes.Select(p => p.Id).ToList();
            var idsSucesosInvalidas = idsSucesosAsociados.Except(idsSucesosExistentes).ToList();

            if (idsSucesosInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Sucesos: {string.Join(", ", idsSucesosInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(Suceso), string.Join(", ", idsSucesosInvalidas));
            }
        }

        // Manejo de DetalleSucesoRelacionado
        var idsExistentes = sucesoRelacionado.DetalleSucesoRelacionados
            .Select(d => d.IdSucesoAsociado)
            .ToList();

        var idsNuevos = request.IdsSucesosAsociados.Except(idsExistentes).ToList();
        var idsAEliminar = idsExistentes.Except(request.IdsSucesosAsociados).ToList();

        // Agregar nuevos sucesos asociados
        foreach (var idNuevo in idsNuevos)
        {
            sucesoRelacionado.DetalleSucesoRelacionados.Add(new DetalleSucesoRelacionado
            {
                IdSucesoAsociado = idNuevo
            });
        }

        // Eliminar los que no se enviaron en el listado
        foreach (var idEliminar in idsAEliminar)
        {
            var detalle = sucesoRelacionado.DetalleSucesoRelacionados
                .FirstOrDefault(d => d.IdSucesoAsociado == idEliminar);

            if (detalle != null)
            {
                sucesoRelacionado.DetalleSucesoRelacionados.Remove(detalle);
            }
        }



        if (request.IdSucesoRelacionado.HasValue && request.IdSucesoRelacionado.Value > 0)
        {
            if (request.IdsSucesosAsociados.Any())
            {
                _unitOfWork.Repository<SucesoRelacionado>().UpdateEntity(sucesoRelacionado);
            }
            else
            {
                _unitOfWork.Repository<SucesoRelacionado>().DeleteEntity(sucesoRelacionado);
            }
        }
        else
        {
            _unitOfWork.Repository<SucesoRelacionado>().AddEntity(sucesoRelacionado);
        }

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar/eliminar los datos de Suceso Relacionado");
        }

        _logger.LogInformation($"{nameof(ManageSucesoRelacionadosCommandHandler)} - END");
        return new ManageSucesoRelacionadoResponse { IdSucesoRelacionado = sucesoRelacionado.Id };
    }
}
