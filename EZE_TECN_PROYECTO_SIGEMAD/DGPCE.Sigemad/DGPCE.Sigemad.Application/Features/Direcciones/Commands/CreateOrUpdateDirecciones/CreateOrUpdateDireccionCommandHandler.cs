using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateImpactoEvoluciones;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
internal class CreateOrUpdateDireccionCommandHandler : IRequestHandler<CreateOrUpdateDireccionCommand, CreateOrUpdateDireccionResponse>
{
    private readonly ILogger<CreateImpactoEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public CreateOrUpdateDireccionCommandHandler(
        ILogger<CreateImpactoEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<CreateOrUpdateDireccionResponse> Handle(CreateOrUpdateDireccionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - BEGIN");

        await ValidateSuceso(request.IdSuceso);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<DireccionCoordinacionEmergencia>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.DireccionCoordinacion);

            DireccionCoordinacionEmergencia direccionCoordinacionEmergencia = await GetOrCreateDireccionCoordinacion(request, registroActualizacion);

            await ValidateTipoDireccionEmergencia(request);

            var direccionesOriginales = direccionCoordinacionEmergencia.Direcciones.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateDireccionDto>(d));
            MapAndSaveDirecciones(request, direccionCoordinacionEmergencia);

            var direccionesParaEliminar = await DeleteLogicalDirecciones(request, direccionCoordinacionEmergencia, registroActualizacion.Id);

            await SaveDireccionCoordinacion(direccionCoordinacionEmergencia);

            await _registroActualizacionService.SaveRegistroActualizacion<
                DireccionCoordinacionEmergencia, Direccion, CreateOrUpdateDireccionDto>(
                registroActualizacion,
                direccionCoordinacionEmergencia,
                ApartadoRegistroEnum.Direccion,
                direccionesParaEliminar, direccionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - END");

            return new CreateOrUpdateDireccionResponse
            {
                IdDireccionCoordinacionEmergencia = direccionCoordinacionEmergencia.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateDireccionCommandHandler");
            throw;
        }
    }

    private async Task ValidateSuceso(int idSuceso)
    {
        var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(idSuceso);
        if (suceso is null || suceso.Borrado)
            throw new NotFoundException(nameof(Suceso), idSuceso);
    }

    /*
    private async Task<RegistroActualizacion> GetOrCreateRegistroActualizacion(CreateOrUpdateDireccionCommand request)
    {
        if (request.IdRegistroActualizacion.HasValue && request.IdRegistroActualizacion.Value > 0)
        {
            var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams
            {
                Id = request.IdRegistroActualizacion.Value,
            });
            var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

            if (registroActualizacion is null)
                throw new NotFoundException(nameof(RegistroActualizacion), request.IdRegistroActualizacion);

            if (registroActualizacion.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.DireccionCoordinacion)
                throw new BadRequestException("El registro de actualización no es de tipo 'Direccion y Coordinacion'");

            return registroActualizacion;
        }

        // Crear nuevo registro de actualización
        return new RegistroActualizacion
        {
            IdTipoRegistroActualizacion = (int)TipoRegistroActualizacionEnum.DireccionCoordinacion,
            IdSuceso = request.IdSuceso,
            TipoEntidad = nameof(DireccionCoordinacionEmergencia)
        };
    }
    */

    private async Task<DireccionCoordinacionEmergencia> GetOrCreateDireccionCoordinacion(CreateOrUpdateDireccionCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            List<int> idsDirecciones = new List<int>();
            List<int> idsCecopi = new List<int>();
            List<int> idsPma = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.Direccion:
                        idsDirecciones.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.CoordinacionCECOPI:
                        idsCecopi.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.CoordinacionPMA:
                        idsPma.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Dirección y Coordinación de Emergencia por IdReferencia
            var direccionCoordinacion = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>()
                .GetByIdWithSpec(new DireccionCoordinacionEmergenciaWithFilteredData(registroActualizacion.IdReferencia, idsDirecciones, idsCecopi, idsPma));

            if (direccionCoordinacion is null || direccionCoordinacion.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de Direccion y Coordinacion");

            return direccionCoordinacion;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new DireccionCoordinacionEmergenciaWithDirecciones(new DireccionCoordinacionEmergenciaParams { IdSuceso = request.IdSuceso });
        var direccionExistente = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetByIdWithSpec(specSuceso);

        return direccionExistente ?? new DireccionCoordinacionEmergencia { IdSuceso = request.IdSuceso };
    }

    private async Task ValidateTipoDireccionEmergencia(CreateOrUpdateDireccionCommand request)
    {
        var idsTipoDireccion = request.Direcciones.Select(d => d.IdTipoDireccionEmergencia).Distinct();
        var tiposDireccionExistentes = await _unitOfWork.Repository<TipoDireccionEmergencia>().GetAsync(td => idsTipoDireccion.Contains(td.Id));

        if (tiposDireccionExistentes.Count() != idsTipoDireccion.Count())
        {
            var idsInvalidos = idsTipoDireccion.Except(tiposDireccionExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(TipoDireccionEmergencia), string.Join(", ", idsInvalidos));
        }
    }

    private void MapAndSaveDirecciones(CreateOrUpdateDireccionCommand request, DireccionCoordinacionEmergencia direccionCoordinacion)
    {
        foreach (var direccionDto in request.Direcciones)
        {
            if (direccionDto.Id.HasValue && direccionDto.Id > 0)
            {
                var direccionExistente = direccionCoordinacion.Direcciones.FirstOrDefault(d => d.Id == direccionDto.Id.Value);
                if (direccionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateDireccionDto>(direccionExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateDireccionDto>(direccionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(direccionDto, direccionExistente);
                        direccionExistente.Borrado = false;
                    }
                }
                else
                {
                    direccionCoordinacion.Direcciones.Add(_mapper.Map<Direccion>(direccionDto));
                }
            }
            else
            {
                direccionCoordinacion.Direcciones.Add(_mapper.Map<Direccion>(direccionDto));
            }
        }
    }

    private async Task<List<int>> DeleteLogicalDirecciones(CreateOrUpdateDireccionCommand request, DireccionCoordinacionEmergencia direccionCoordinacion, int idRegistroActualizacion)
    {
        if (direccionCoordinacion.Id > 0)
        {
            var idsEnRequest = request.Direcciones.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var direccionesParaEliminar = direccionCoordinacion.Direcciones
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (direccionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsDireccionesParaEliminar = direccionesParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsDireccionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Direccion);

            foreach (var direccion in direccionesParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == direccion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La dirección con ID {direccion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<Direccion>().DeleteEntity(direccion);
            }

            return idsDireccionesParaEliminar;
        }

        return new List<int>();
    }

    private async Task SaveDireccionCoordinacion(DireccionCoordinacionEmergencia direccionCoordinacion)
    {
        if (direccionCoordinacion.Id > 0)
        {
            _unitOfWork.Repository<DireccionCoordinacionEmergencia>().UpdateEntity(direccionCoordinacion);
        }
        else
        {
            _unitOfWork.Repository<DireccionCoordinacionEmergencia>().AddEntity(direccionCoordinacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la Dirección y Coordinación de Emergencia");
    }

    /*
    private async Task SaveRegistroActualizacion(
        RegistroActualizacion registroActualizacion,
        DireccionCoordinacionEmergencia direccionCoordinacion,
        List<int> direccionesParaEliminar,
        Dictionary<int, CreateOrUpdateDireccionDto> direccionesOriginales)
    {
        registroActualizacion.IdReferencia = direccionCoordinacion.Id;
        var nuevasDireccionesIds = new List<int>();

        foreach (var direccion in direccionCoordinacion.Direcciones)
        {
            var estado = GetEstadoRegistro(direccion, registroActualizacion.DetallesRegistro, direccionesOriginales, direccionesParaEliminar);

            var detalleExistente = registroActualizacion.DetallesRegistro.FirstOrDefault(d => d.IdReferencia == direccion.Id);

            if (detalleExistente != null)
            {
                if (estado == EstadoRegistroEnum.Permanente) continue;

                detalleExistente.IdEstadoRegistro = estado;
                //_unitOfWork.Repository<DetalleRegistroActualizacion>().UpdateEntity(detalleExistente);
            }
            else
            {
                registroActualizacion.DetallesRegistro.Add(new DetalleRegistroActualizacion
                {
                    IdApartadoRegistro = (int)ApartadoRegistroEnum.Direccion,
                    IdReferencia = direccion.Id,
                    IdEstadoRegistro = estado
                });
                if (estado == EstadoRegistroEnum.Creado)
                {
                    // Registrar las nuevas direcciones
                    nuevasDireccionesIds.Add(direccion.Id);
                }
            }

        }

        // Guardar el registro de actualización
        if (registroActualizacion.Id > 0)
        {
            // Llamar a ReflectNewDireccionesInFutureRegistros para propagar las nuevas direcciones a registros posteriores
            await ReflectNewDireccionesInFutureRegistros(registroActualizacion, nuevasDireccionesIds);

            _unitOfWork.Repository<RegistroActualizacion>().UpdateEntity(registroActualizacion);
        }
        else
        {
            _unitOfWork.Repository<RegistroActualizacion>().AddEntity(registroActualizacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar registros de actualizaciones");
    }

    private EstadoRegistroEnum GetEstadoRegistro(
    Direccion direccion,
    RegistroActualizacion registroActualizacion,
    Dictionary<int, CreateOrUpdateDireccionDto> direccionesOriginales,
    List<int> direccionesParaEliminar)
    {
        if (direccionesParaEliminar.Contains(direccion.Id))
        {
            return EstadoRegistroEnum.Eliminado;
        }

        if (!direccionesOriginales.ContainsKey(direccion.Id))
        {
            return EstadoRegistroEnum.Creado;
        }

        var copiaOriginal = direccionesOriginales[direccion.Id];
        var copiaNueva = _mapper.Map<CreateOrUpdateDireccionDto>(direccion);

        if (copiaOriginal.Equals(copiaNueva))
        {
            return EstadoRegistroEnum.Permanente;
        }

        return EstadoRegistroEnum.Modificado;
    }

    private EstadoRegistroEnum GetEstadoRegistro(
    Direccion direccion,
    IEnumerable<DetalleRegistroActualizacion> detallesPrevios,
    Dictionary<int, CreateOrUpdateDireccionDto> direccionesOriginales,
    List<int> direccionesParaEliminar)
    {
        if (direccionesParaEliminar.Contains(direccion.Id))
        {
            return EstadoRegistroEnum.Eliminado;
        }

        if (!direccionesOriginales.ContainsKey(direccion.Id))
        {
            return EstadoRegistroEnum.Creado;
        }

        var detallePrevio = detallesPrevios.FirstOrDefault(d => d.IdReferencia == direccion.Id);

        var copiaOriginal = direccionesOriginales[direccion.Id];
        var copiaNueva = _mapper.Map<CreateOrUpdateDireccionDto>(direccion);

        if (detallePrevio != null)
        {
            if (!copiaOriginal.Equals(copiaNueva))
            {
                if (detallePrevio.IdEstadoRegistro == EstadoRegistroEnum.Creado)
                    return EstadoRegistroEnum.CreadoYModificado;
                return EstadoRegistroEnum.Modificado;
            }
            return EstadoRegistroEnum.Permanente;
        }

        if (copiaOriginal.Equals(copiaNueva))
        {
            return EstadoRegistroEnum.Permanente;
        }

        return EstadoRegistroEnum.Modificado;
    }

    private async Task ReflectNewDireccionesInFutureRegistros(
        RegistroActualizacion registroActualizacion,
        List<int> nuevasDireccionesIds)
    {
        if (!nuevasDireccionesIds.Any()) return;

        // Buscar registros de actualización posteriores al actual

        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams
        {
            IdMinimo = registroActualizacion.Id,
            IdSuceso = registroActualizacion.IdSuceso,
            IdTipoRegistroActualizacion = (int)TipoRegistroActualizacionEnum.DireccionCoordinacion
        });
        var registrosPosteriores = await _unitOfWork.Repository<RegistroActualizacion>().GetAllWithSpec(spec);

        foreach (var registroPosterior in registrosPosteriores)
        {
            bool seActualizoRegistroPosterior = false;
            // Obtener los detalles previos de cada registro
            //var detallesPrevios = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
            //    .GetAsync(d => d.IdRegistroActualizacion == registroPosterior.Id);

            var detallesPrevios = registroPosterior.DetallesRegistro;

            foreach (var idDireccion in nuevasDireccionesIds)
            {
                // Si la dirección ya existe en el registro, no la agregamos nuevamente
                if (detallesPrevios.Any(d => d.IdReferencia == idDireccion)) continue;

                // Agregar la dirección con estado "Creado en Registro Anterior"
                var nuevoDetalle = new DetalleRegistroActualizacion
                {
                    IdRegistroActualizacion = registroPosterior.Id,
                    IdApartadoRegistro = (int)ApartadoRegistroEnum.Direccion,
                    IdReferencia = idDireccion,
                    IdEstadoRegistro = EstadoRegistroEnum.CreadoEnRegistroAnterior
                };

                registroPosterior.DetallesRegistro.Add(nuevoDetalle);
                seActualizoRegistroPosterior = true;
            }

            if (seActualizoRegistroPosterior)
                _unitOfWork.Repository<RegistroActualizacion>().UpdateEntity(registroPosterior);
        }
    }
    */
}
