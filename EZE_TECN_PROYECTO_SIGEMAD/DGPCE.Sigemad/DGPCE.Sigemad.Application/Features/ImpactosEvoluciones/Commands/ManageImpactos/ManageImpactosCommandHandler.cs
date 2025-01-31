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

        var evolucion = await GetOrCreateEvolucionAsync(request);
        await ValidateImpactosClasificadosAsync(request);
        await ValidateTiposDanioAsync(request);

        MapAndManageImpactos(request, evolucion);
        await DeleteLogicalActivaciones(request, evolucion);

        await SaveEvolucionAsync(request, evolucion);

        _logger.LogInformation($"{nameof(CreateImpactoEvolucionCommandHandler)} - END");
        return new ManageImpactoResponse { IdEvolucion = evolucion.Id };
    }

    private async Task<Evolucion> GetOrCreateEvolucionAsync(ManageImpactosCommand request)
    {
        if (request.IdEvolucion.HasValue && request.IdEvolucion.Value > 0)
        {
            var spec = new EvolucionByIdWithImpactosSpecification(request.IdEvolucion.Value);
            var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(spec);
            if (evolucion is null)
            {
                _logger.LogWarning($"request.IdEvolucion: {request.IdEvolucion}, no encontrado");
                throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
            }
            return evolucion;
        }
        else
        {
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (suceso is null || suceso.Borrado)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            return new Evolucion { IdSuceso = request.IdSuceso };
        }
    }

    private async Task ValidateImpactosClasificadosAsync(ManageImpactosCommand request)
    {
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
    }

    private async Task ValidateTiposDanioAsync(ManageImpactosCommand request)
    {
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
    }

    private void MapAndManageImpactos(ManageImpactosCommand request, Evolucion evolucion)
    {
        foreach (var impactoDto in request.Impactos)
        {
            if (impactoDto.Id.HasValue && impactoDto.Id > 0)
            {
                var impacto = evolucion.Impactos.FirstOrDefault(d => d.Id == impactoDto.Id.Value);
                if (impacto != null)
                {
                    _mapper.Map(impactoDto, impacto);
                    impacto.Borrado = false;
                }
                else
                {
                    var nuevoImpacto = _mapper.Map<ImpactoEvolucion>(impactoDto);
                    nuevoImpacto.Id = 0;
                    evolucion.Impactos.Add(nuevoImpacto);
                }
            }
            else
            {
                var nuevoImpacto = _mapper.Map<ImpactoEvolucion>(impactoDto);
                nuevoImpacto.Id = 0;
                evolucion.Impactos.Add(nuevoImpacto);
            }
        }
    }

    private async Task DeleteLogicalActivaciones(ManageImpactosCommand request, Evolucion evolucion)
    {
        if (request.IdEvolucion.HasValue && request.IdEvolucion.Value > 0)
        {
            var idsEnRequest = request.Impactos
                .Where(d => d.Id.HasValue && d.Id > 0)
            .Select(d => d.Id)
            .ToList();

            var impactosParaEliminar = evolucion.Impactos
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            foreach (var impacto in impactosParaEliminar)
            {
                _unitOfWork.Repository<ImpactoEvolucion>().DeleteEntity(impacto);
            }
        }
    }

    private async Task SaveEvolucionAsync(ManageImpactosCommand request, Evolucion evolucion)
    {
        if (request.IdEvolucion.HasValue && request.IdEvolucion.Value > 0)
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
    }
}
