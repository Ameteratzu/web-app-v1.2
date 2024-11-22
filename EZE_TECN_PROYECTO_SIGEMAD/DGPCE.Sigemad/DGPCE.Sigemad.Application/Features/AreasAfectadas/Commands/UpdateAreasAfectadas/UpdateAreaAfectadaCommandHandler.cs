using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands.UpdateAreasAfectadas;
internal class UpdateAreaAfectadaCommandHandler : IRequestHandler<UpdateAreaAfectadaCommand, UpdateAreaAfectadaResponse>
{
    private readonly ILogger<UpdateAreaAfectadaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IGeometryValidator _geometryValidator;

    public UpdateAreaAfectadaCommandHandler(
        ILogger<UpdateAreaAfectadaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IGeometryValidator geometryValidator,
        IMapper mapper
        )
    {
        _logger = logger;
        _geometryValidator = geometryValidator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UpdateAreaAfectadaResponse> Handle(UpdateAreaAfectadaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateAreaAfectadaCommandHandler) + " - BEGIN");

        // Obtener la evolución existente
        var evolucionSpec = new UpdateEvolucionWithAreaAfectadaSpecification(request.IdEvolucion);
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);
        if (evolucion is null)
        {
            _logger.LogWarning($"idEvolucion: {request.IdEvolucion}, no encontrado");
            throw new NotFoundException(nameof(Evolucion), request.IdEvolucion);
        }

        // Validar Provincias
        var idsProvincias = request.AreasAfectadas.Select(a => a.IdProvincia).Distinct().ToList();
        var provinciasExistentes = await _unitOfWork.Repository<Provincia>()
            .GetAsync(p => idsProvincias.Contains(p.Id) && p.Borrado == false);

        if (provinciasExistentes.Count() != idsProvincias.Count())
        {
            var idsProvinciasExistentes = provinciasExistentes.Select(p => p.Id).ToList();
            var idsProvinciasInvalidas = idsProvincias.Except(idsProvinciasExistentes).ToList();

            if (idsProvinciasInvalidas.Any())
            {
                _logger.LogWarning($"Las siguientes Id's de provincias: {string.Join(", ", idsProvinciasInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(Provincia), string.Join(", ", idsProvinciasInvalidas));
            }
        }

        // Validar Municipios
        var idsMunicipios = request.AreasAfectadas.Select(a => a.IdMunicipio).Distinct().ToList();
        var municipiosExistentes = await _unitOfWork.Repository<Municipio>()
            .GetAsync(p => idsMunicipios.Contains(p.Id) && p.Borrado == false);

        if (municipiosExistentes.Count() != idsMunicipios.Count())
        {
            var idsMunicipiosExistentes = municipiosExistentes.Select(p => p.Id).ToList();
            var idsMunicipiosInvalidos = idsMunicipios.Except(idsMunicipiosExistentes).ToList();

            if (idsMunicipiosInvalidos.Any())
            {
                _logger.LogWarning($"Las siguientes Id's de municipios: {string.Join(", ", idsMunicipiosInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(Municipio), string.Join(", ", idsMunicipiosInvalidos));
            }
        }

        // Validar Entidades Menores (opcional)
        var idsEntidadMenor = request.AreasAfectadas
            .Where(a => a.IdEntidadMenor.HasValue)
            .Select(a => a.IdEntidadMenor.Value)
            .Distinct()
            .ToList();

        if (idsEntidadMenor.Any())
        {
            var entidadMenorExistentes = await _unitOfWork.Repository<EntidadMenor>()
                .GetAsync(p => idsEntidadMenor.Contains(p.Id) && p.Borrado == false);

            if (entidadMenorExistentes.Count() != idsEntidadMenor.Count())
            {
                var idsEntidadMenorExistentes = entidadMenorExistentes.Select(p => p.Id).ToList();
                var idsEntidadMenorInvalidas = idsEntidadMenor.Except(idsEntidadMenorExistentes).ToList();

                if (idsEntidadMenorInvalidas.Any())
                {
                    _logger.LogWarning($"Las siguientes Id's de entidad menor: {string.Join(", ", idsEntidadMenorInvalidas)}, no se encontraron");
                    throw new NotFoundException(nameof(EntidadMenor), string.Join(", ", idsEntidadMenorInvalidas));
                }
            }
        }



        // Obtener las áreas afectadas actuales de la evolución
        var areasAfectadasActuales = evolucion.AreaAfectadas.ToList();

        // IDs de las áreas afectadas enviadas en la petición
        var idsAreasAfectadasEnviadas = request.AreasAfectadas
            .Where(a => a.Id.HasValue)
            .Select(a => a.Id.Value)
            .ToList();

        // Determinar las áreas afectadas a eliminar (las que no están en la lista proporcionada)
        var areasAfectadasAEliminar = areasAfectadasActuales
            .Where(a => !idsAreasAfectadasEnviadas.Contains(a.Id) && a.Borrado == false)
            .ToList();

        // Marcar las áreas afectadas como eliminadas
        foreach (var areaAEliminar in areasAfectadasAEliminar)
        {
            _unitOfWork.Repository<AreaAfectada>().DeleteEntity(areaAEliminar);
        }

        //-------------------------------------------------------------
        foreach (var areaAfectadaDto in request.AreasAfectadas)
        {
            if (areaAfectadaDto.Id.HasValue)
            {
                // Actualizar Área Afectada existente
                var areaExistente = areasAfectadasActuales.FirstOrDefault(a => a.Id == areaAfectadaDto.Id.Value);
                if (areaExistente != null)
                {
                    _mapper.Map(areaAfectadaDto, areaExistente);
                    areaExistente.Borrado = false;
                }
                else
                {
                    _logger.LogWarning($"idAreaAfectada: {areaAfectadaDto.Id.Value}, no encontrado");
                    throw new NotFoundException(nameof(AreaAfectada), areaAfectadaDto.Id.Value);
                }
            }
            else
            {
                // Crear nueva Área Afectada
                var nuevaAreaAfectada = _mapper.Map<AreaAfectada>(areaAfectadaDto);
                evolucion.AreaAfectadas.Add(nuevaAreaAfectada);
            }
        }


        // Actualizar la evolución en la base de datos
        _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);
        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo actualizar las áreas afectadas");
        }

        _logger.LogInformation(nameof(UpdateAreaAfectadaCommandHandler) + " - END");

        return new UpdateAreaAfectadaResponse { IdEvolucion = evolucion.Id };
    }
}
