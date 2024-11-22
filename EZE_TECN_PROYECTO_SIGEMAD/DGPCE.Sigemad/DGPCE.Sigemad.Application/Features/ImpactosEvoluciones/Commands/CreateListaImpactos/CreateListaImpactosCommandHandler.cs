using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactos;
public class CreateListaImpactosCommandHandler : IRequestHandler<CreateListaImpactosCommand, CreateListaImpactosResponse>
{
    private readonly ILogger<CreateImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateListaImpactosCommandHandler(
        ILogger<CreateImpactoEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateListaImpactosResponse> Handle(CreateListaImpactosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateImpactoEvolucionCommandHandler)} - BEGIN");

        Evolucion evolucion;

        // Verificar si el IdEvolucion es proporcionado, si no, crear una nueva evolución
        if (request.IdEvolucion.HasValue)
        {
            evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdAsync(request.IdEvolucion.Value);
            if (evolucion is null || evolucion.Borrado)
            {
                _logger.LogWarning($"request.IdEvolucion: {request.IdEvolucion}, no encontrado");
                throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
            }
        }
        else
        {
            // Validar si el IdIncendio es válido
            var incendio = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.IdIncendio);
            if (incendio is null || incendio.Borrado)
            {
                _logger.LogWarning($"request.IdIncendio: {request.IdIncendio}, no encontrado");
                throw new NotFoundException(nameof(Incendio), request.IdIncendio);
            }

            // Crear nueva evolución si no se proporciona IdEvolucion
            evolucion = new Evolucion
            {
                IdIncendio = request.IdIncendio
            };
        }

        // Validar los IdImpactoClasificado de los impactos en el listado
        var idsImpactosClasificados = request.Impactos.Select(ie => ie.IdImpactoClasificado).Distinct();
        var impactosClasificadosExistentes = await _unitOfWork.Repository<ImpactoClasificado>().GetAsync(ic => idsImpactosClasificados.Contains(ic.Id));

        if (impactosClasificadosExistentes.Count() != idsImpactosClasificados.Count())
        {
            var idsImpactosClasificadosExistentes = impactosClasificadosExistentes.Select(ic => ic.Id).ToList();
            var idsImpactosClasificadosInvalidos = idsImpactosClasificados.Except(idsImpactosClasificadosExistentes).ToList();

            if (idsImpactosClasificadosInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de impacto clasificado: {string.Join(", ", idsImpactosClasificadosInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(ImpactoClasificado), string.Join(", ", idsImpactosClasificadosInvalidos));
            }
        }

        // VALIDACIÓN DE LOS ID EN EL LISTADO DE IMPACTOS DE EVOLUCIÓN
        var idsTipoDanio = request.Impactos
            .Where(i => i.IdTipoDanio.HasValue)
            .Select(i => i.IdTipoDanio.Value)
            .Distinct()
            .ToList();

        if (idsTipoDanio.Any())
        {
            var tiposDanioExistentes = await _unitOfWork.Repository<TipoDanio>().GetAsync(t => idsTipoDanio.Contains(t.Id));
            if (tiposDanioExistentes.Count() != idsTipoDanio.Count())
            {
                var idsTipoDanioExistentes = tiposDanioExistentes.Select(t => t.Id).ToList();
                var idsTipoDanioInvalidos = idsTipoDanio.Except(idsTipoDanioExistentes).ToList();

                if (idsTipoDanioInvalidos.Any())
                {
                    _logger.LogWarning($"Los siguientes Id's de tipos de daño: {string.Join(", ", idsTipoDanioInvalidos)}, no se encontraron");
                    throw new NotFoundException(nameof(TipoDanio), string.Join(", ", idsTipoDanioInvalidos));
                }
            }
        }

        // Mapear y guardar los impactos de evolución
        evolucion.Impactos = _mapper.Map<ICollection<ImpactoEvolucion>>(request.Impactos);

        if (request.IdEvolucion.HasValue)
        {
            _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);
        }
        else
        {
            _unitOfWork.Repository<Evolucion>().AddEntity(evolucion);
        }

        var saveResult = await _unitOfWork.Complete();
        if (saveResult <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar nueva evolución");
        }

        _logger.LogInformation($"{nameof(CreateImpactoEvolucionCommandHandler)} - END");
        return new CreateListaImpactosResponse { IdEvolucion = evolucion.Id };
    }
}
