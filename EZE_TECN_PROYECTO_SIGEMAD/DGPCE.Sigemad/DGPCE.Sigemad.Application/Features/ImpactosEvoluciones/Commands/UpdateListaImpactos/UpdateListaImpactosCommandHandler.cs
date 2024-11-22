using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.UpdateListaImpactos;
public class UpdateListaImpactosCommandHandler : IRequestHandler<UpdateListaImpactosCommand, UpdateListaImpactosResponse>
{
    private readonly ILogger<UpdateListaImpactosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateListaImpactosCommandHandler(
        ILogger<UpdateListaImpactosCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateListaImpactosResponse> Handle(UpdateListaImpactosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(UpdateListaImpactosCommandHandler)} - BEGIN");

        // Obtener la evolución existente
        var evolucionSpec = new UpdateEvolucionWithImpactosSpecification(request.IdEvolucion);
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);
        if (evolucion is null || evolucion.Borrado)
        {
            _logger.LogWarning($"request.IdEvolucion: {request.IdEvolucion}, no encontrado");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
        }

        // Validar los IdImpactoClasificado de los impactos en el listado
        var idsImpactosClasificados = request.Impactos
            .Select(ie => ie.IdImpactoClasificado)
            .Distinct()
            .ToList();

        var impactosClasificadosExistentes = await _unitOfWork.Repository<ImpactoClasificado>()
            .GetAsync(ic => idsImpactosClasificados.Contains(ic.Id));

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

        // Validar los IdTipoDanio en el listado de impactos
        var idsTipoDanio = request.Impactos
            .Where(i => i.IdTipoDanio.HasValue)
            .Select(i => i.IdTipoDanio.Value)
            .Distinct()
            .ToList();

        if (idsTipoDanio.Any())
        {
            var tiposDanioExistentes = await _unitOfWork.Repository<TipoDanio>()
                .GetAsync(t => idsTipoDanio.Contains(t.Id));

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


        // Obtener la lista de impactos actuales de la evolución
        var impactosActuales = evolucion.Impactos.ToList();

        // Manejo de impactos proporcionados en la solicitud
        foreach (var impactoDto in request.Impactos)
        {
            if (impactoDto.Id.HasValue && impactoDto.Id.Value > 0)
            {
                // Si el Id existe, buscar el impacto en la lista actual
                var impactoExistente = impactosActuales.FirstOrDefault(i => i.Id == impactoDto.Id.Value);
                if (impactoExistente != null)
                {
                    // Actualizar el impacto existente
                    _mapper.Map(impactoDto, impactoExistente);
                }
                else
                {
                    _logger.LogWarning($"ImpactoEvolucion con Id {impactoDto.Id.Value} no encontrado en la evolución {request.IdEvolucion}");
                    throw new NotFoundException(nameof(ImpactoEvolucion), impactoDto.Id.Value);
                }
            }
            else
            {
                // Si el Id no está presente, crear un nuevo impacto
                var nuevoImpacto = _mapper.Map<ImpactoEvolucion>(impactoDto);
                evolucion.Impactos.Add(nuevoImpacto);
            }
        }

        // Manejo de eliminación de impactos no presentes en la solicitud
        var idsProporcionados = request.Impactos
            .Where(i => i.Id.HasValue)
            .Select(i => i.Id.Value)
            .ToList();

        var impactosAEliminar = impactosActuales
            .Where(i => !idsProporcionados.Contains(i.Id))
            .ToList();

        foreach (var impacto in impactosAEliminar)
        {
            _unitOfWork.Repository<ImpactoEvolucion>().DeleteEntity(impacto);
        }

        // Guardar cambios en la base de datos
        _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);

        var saveResult = await _unitOfWork.Complete();
        if (saveResult <= 0)
        {
            throw new Exception("No se pudo actualizar la evolución con los impactos proporcionados");
        }

        _logger.LogInformation($"{nameof(UpdateListaImpactosCommandHandler)} - END");
        return new UpdateListaImpactosResponse { IdEvolucion = evolucion.Id };
    }
}
