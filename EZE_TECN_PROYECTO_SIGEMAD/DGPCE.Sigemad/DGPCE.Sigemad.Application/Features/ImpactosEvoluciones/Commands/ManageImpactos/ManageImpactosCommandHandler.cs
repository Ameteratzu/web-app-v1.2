using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactos;
public class ManageImpactosCommandHandler : IRequestHandler<ManageImpactosCommand, ManageImpactoResponse>
{
    private readonly ILogger<CreateImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManageImpactosCommandHandler(
        ILogger<CreateImpactoEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ManageImpactoResponse> Handle(ManageImpactosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateImpactoEvolucionCommandHandler)} - BEGIN");

        Evolucion evolucion;

        // Verificar si el IdEvolucion es proporcionado, si no, crear una nueva evolución
        if (request.IdEvolucion.HasValue)
        {
            var spec = new EvolucionByIdWithImpactosSpecification(request.IdEvolucion.Value);
            evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(spec);
            if (evolucion is null)
            {
                _logger.LogWarning($"request.IdEvolucion: {request.IdEvolucion}, no encontrado");
                throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
            }
        }
        else
        {
            // Validar si el IdSuceso es válido
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (suceso is null || suceso.Borrado)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            // Crear nueva evolución si no se proporciona IdEvolucion
            evolucion = new Evolucion
            {
                IdSuceso = request.IdSuceso
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

        // Mapear y actualizar/crear las direcciones de la emergencia
        foreach (var impactoDto in request.Impactos)
        {
            if (impactoDto.Id.HasValue && impactoDto.Id > 0)
            {
                var direccion = evolucion.Impactos.FirstOrDefault(d => d.Id == impactoDto.Id.Value);
                if (direccion != null)
                {
                    // Actualizar datos existentes
                    _mapper.Map(impactoDto, direccion);
                    direccion.Borrado = false;
                }
                else
                {
                    // Crear nueva dirección
                    var nuevoImpacto = _mapper.Map<ImpactoEvolucion>(impactoDto);
                    nuevoImpacto.Id = 0;
                    evolucion.Impactos.Add(nuevoImpacto);
                }
            }
            else
            {
                // Crear nueva dirección
                var nuevoImpacto = _mapper.Map<ImpactoEvolucion>(impactoDto);
                nuevoImpacto.Id = 0;
                evolucion.Impactos.Add(nuevoImpacto);
            }
        }

        // Eliminar lógicamente las direcciones que no están presentes en el request solo si IdDireccionCoordinacionEmergencia es existente
        if (request.IdEvolucion.HasValue)
        {
            // Solo las direcciones con Id existente (no nuevas)
            var idsEnRequest = request.Impactos
                .Where(d => d.Id.HasValue && d.Id > 0)
            .Select(d => d.Id)
            .ToList();

            var impactosParaEliminar = evolucion.Impactos
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id)) // Solo las direcciones que ya existen en la base de datos y no están en el request
                .ToList();
            foreach (var impacto in impactosParaEliminar)
            {
                _unitOfWork.Repository<ImpactoEvolucion>().DeleteEntity(impacto);
            }
        }



        // Guardar los impactos de evolución
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
        return new ManageImpactoResponse { IdEvolucion = evolucion.Id };
    }
}
