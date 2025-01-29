﻿using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.MovilizacionMedios.Commands.ManageMovilizacionMedios;
public class ManageMovilizacionMediosCommandHandler : IRequestHandler<ManageMovilizacionMediosCommand, ManageMovilizacionMediosResponse>
{
    private readonly ILogger<ManageMovilizacionMediosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private const string ARCHIVOS_PATH = "movilizacion-medios";

    public ManageMovilizacionMediosCommandHandler(
        ILogger<ManageMovilizacionMediosCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<ManageMovilizacionMediosResponse> Handle(ManageMovilizacionMediosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageMovilizacionMediosCommandHandler)} - BEGIN");

        var actuacionRelevante = await GetOrCreateDocumentacionAsync(request);

        await ValidateIdsAsync(request);
        await ValidarFlujoPasos(request);
        await MapAndManageMovilizaciones(request, actuacionRelevante);
        await PersistDocumentacionAsync(request, actuacionRelevante);

        _logger.LogInformation($"{nameof(ManageMovilizacionMediosCommandHandler)} - END");
        return new ManageMovilizacionMediosResponse { IdActuacionRelevante = actuacionRelevante.Id };
    }

    private async Task MapAndManageMovilizaciones(ManageMovilizacionMediosCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        foreach (var movilizacionDto in request.Movilizaciones)
        {
            if (movilizacionDto.Id.HasValue && movilizacionDto.Id > 0)
            {
                var movilizacionExistente = actuacionRelevante.MovilizacionMedios.FirstOrDefault(c => c.Id == movilizacionDto.Id.Value);

                if (movilizacionExistente != null)
                {
                    //_mapper.Map(movilizacionDto, movilizacionExistente);
                    movilizacionExistente.Borrado = false;

                    foreach (var pasoDto in movilizacionDto.Pasos)
                    {
                        var ejecucionPasoExistente = movilizacionExistente.Pasos
                            .FirstOrDefault(p => p.Id == pasoDto.Id);

                        if (ejecucionPasoExistente != null)
                        {
                            // 🔹 Si el paso existe, actualizarlo
                            //_mapper.Map(pasoDto, ejecucionPasoExistente.);
                            ejecucionPasoExistente.Borrado = false;
                            ActualizarPasoEspecifico(ejecucionPasoExistente, pasoDto);
                        }
                        else
                        {
                            // 🔹 Si el paso no existe, crearlo
                            var nuevoEjecucionPaso = new EjecucionPaso
                            {
                                IdPasoMovilizacion = (int)pasoDto.TipoPaso,
                            };

                            await AgregarPasoEspecifico(nuevoEjecucionPaso, pasoDto);

                            movilizacionExistente.Pasos.Add(nuevoEjecucionPaso);
                        }
                    }
                }
                else
                {
                    var nuevaMovilizacion = await CreateMovilizacion(movilizacionDto);
                    actuacionRelevante.MovilizacionMedios.Add(nuevaMovilizacion);
                }
            }
            else
            {
                var nuevaMovilizacion = await CreateMovilizacion(movilizacionDto);
                actuacionRelevante.MovilizacionMedios.Add(nuevaMovilizacion);
            }
        }

        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante > 0)
        {
            var idsEnRequest = request.Movilizaciones
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var detallesMovilizacionesParaEliminar = actuacionRelevante.MovilizacionMedios
                .Where(c => c.Id > 0 && c.Borrado == false && !idsEnRequest.Contains(c.Id))
                .ToList();

            foreach (var detalleAEliminar in detallesMovilizacionesParaEliminar)
            {
                foreach (var pasoAEliminar in detalleAEliminar.Pasos)
                {
                    _unitOfWork.Repository<EjecucionPaso>().DeleteEntity(pasoAEliminar);
                }
                _unitOfWork.Repository<MovilizacionMedio>().DeleteEntity(detalleAEliminar);
            }
        }
    }

    private async Task<MovilizacionMedio> CreateMovilizacion(MovilizacionMedioDto movilizacionDto)
    {
        var nuevaMovilizacion = new MovilizacionMedio
        {
            Solicitante = movilizacionDto.Solicitante,
            Pasos = new List<EjecucionPaso>(),
        };

        foreach (var pasoDto in movilizacionDto.Pasos)
        {
            var ejecucionPaso = new EjecucionPaso
            {
                IdPasoMovilizacion = (int)pasoDto.TipoPaso,
            };

            await AgregarPasoEspecifico(ejecucionPaso, pasoDto);
            nuevaMovilizacion.Pasos.Add(ejecucionPaso);
        }

        return nuevaMovilizacion;
    }

    private void ActualizarPasoEspecifico(EjecucionPaso ejecucionPaso, DatosPasoBase pasoDto)
    {
        switch (pasoDto)
        {
            case ManageSolicitudMedioDto solicitud:
                if (ejecucionPaso.SolicitudMedio != null)
                {
                    _mapper.Map(solicitud, ejecucionPaso.SolicitudMedio);
                    ejecucionPaso.SolicitudMedio.Borrado = false;
                }
                break;

            case ManageTramitacionMedioDto tramitacion:
                if (ejecucionPaso.TramitacionMedio != null)
                {
                    _mapper.Map(tramitacion, ejecucionPaso.TramitacionMedio);
                    ejecucionPaso.TramitacionMedio.Borrado = false;
                }
                break;

            case ManageAportacionMedioDto aportacion:
                if (ejecucionPaso.AportacionMedio != null)
                {
                    _mapper.Map(aportacion, ejecucionPaso.AportacionMedio);
                    ejecucionPaso.AportacionMedio.Borrado = false;
                }
                break;

            case ManageCancelacionMedioDto cancelacion:
                if (ejecucionPaso.CancelacionMedio != null)
                {
                    _mapper.Map(cancelacion, ejecucionPaso.CancelacionMedio);
                    ejecucionPaso.CancelacionMedio.Borrado = false;
                }
                break;

            case ManageOfrecimientoMedioDto ofrecimiento:
                if (ejecucionPaso.OfrecimientoMedio != null)
                {
                    _mapper.Map(ofrecimiento, ejecucionPaso.OfrecimientoMedio);
                    ejecucionPaso.OfrecimientoMedio.Borrado = false;
                }
                break;

            case ManageDespliegueMedioDto despliegue:
                if (ejecucionPaso.DespliegueMedio != null)
                {
                    _mapper.Map(despliegue, ejecucionPaso.DespliegueMedio);
                    ejecucionPaso.DespliegueMedio.Borrado = false;
                }
                break;

            case ManageFinIntervencionMedioDto finIntervencion:
                if (ejecucionPaso.FinIntervencionMedio != null)
                {
                    _mapper.Map(finIntervencion, ejecucionPaso.FinIntervencionMedio);
                    ejecucionPaso.FinIntervencionMedio.Borrado = false;
                }
                break;

            case ManageLlegadaBaseMedioDto llegadaBase:
                if (ejecucionPaso.LlegadaBaseMedio != null)
                {
                    _mapper.Map(llegadaBase, ejecucionPaso.LlegadaBaseMedio);
                    ejecucionPaso.LlegadaBaseMedio.Borrado = false;
                }
                break;
        }
    }


    private async Task AgregarPasoEspecifico(EjecucionPaso ejecucionPaso, DatosPasoBase pasoDto)
    {
        switch (pasoDto)
        {
            case ManageSolicitudMedioDto solicitud:
                ejecucionPaso.SolicitudMedio = new SolicitudMedio
                {
                    IdProcedenciaMedio = solicitud.IdProcedenciaMedio,
                    AutoridadSolicitante = solicitud.AutoridadSolicitante,
                    FechaHoraSolicitud = solicitud.FechaHoraSolicitud,
                    Descripcion = solicitud.Descripcion,
                    Observaciones = solicitud.Observaciones,
                    Archivo = await MapArchivo(solicitud, null)
                };
                break;
            case ManageTramitacionMedioDto tramitacion:
                ejecucionPaso.TramitacionMedio = new TramitacionMedio
                {
                    IdDestinoMedio = tramitacion.IdDestinoMedio,
                    TitularMedio = tramitacion.TitularMedio,
                    FechaHoraTramitacion = tramitacion.FechaHoraTramitacion,
                    Descripcion = tramitacion.Descripcion,
                    Observaciones = tramitacion.Observaciones
                };
                break;
            case ManageCancelacionMedioDto cancelacion:
                ejecucionPaso.CancelacionMedio = new CancelacionMedio
                {
                    Motivo = cancelacion.Motivo,
                    FechaHoraCancelacion = cancelacion.FechaHoraCancelacion,
                };
                break;
            case ManageDespliegueMedioDto despliegue:
                ejecucionPaso.DespliegueMedio = new DespliegueMedio
                {
                    IdCapacidad = despliegue.IdCapacidad,
                    MedioNoCatalogado = despliegue.MedioNoCatalogado,
                    FechaHoraDespliegue = despliegue.FechaHoraDespliegue,
                    FechaHoraInicioIntervencion = despliegue.FechaHoraInicioIntervencion,
                    Observaciones = despliegue.Observaciones
                };
                break;
            case ManageOfrecimientoMedioDto ofrecimiento:
                ejecucionPaso.OfrecimientoMedio = new OfrecimientoMedio
                {
                    TitularMedio = ofrecimiento.TitularMedio,
                    GestionCECOD = ofrecimiento.GestionCECOD,
                    FechaHoraOfrecimiento = ofrecimiento.FechaHoraOfrecimiento,
                    FechaHoraDisponibilidad = ofrecimiento.FechaHoraDisponibilidad,
                    Descripcion = ofrecimiento.Descripcion,
                    Observaciones = ofrecimiento.Observaciones
                };
                break;
            case ManageFinIntervencionMedioDto finIntervencion:
                ejecucionPaso.FinIntervencionMedio = new FinIntervencionMedio
                {
                    IdCapacidad = finIntervencion.IdCapacidad,
                    MedioNoCatalogado = finIntervencion.MedioNoCatalogado,
                    FechaHoraInicioIntervencion = finIntervencion.FechaHoraInicioIntervencion,
                    Observaciones = finIntervencion.Observaciones
                };
                break;
            case ManageAportacionMedioDto aportacion:
                ejecucionPaso.AportacionMedio = new AportacionMedio
                {
                    IdTipoAdministracion = aportacion.IdTipoAdministracion,
                    IdCapacidad = aportacion.IdCapacidad,
                    TitularMedio = aportacion.TitularMedio,
                    MedioNoCatalogado = aportacion.MedioNoCatalogado,
                    FechaHoraAportacion = aportacion.FechaHoraAportacion,
                    Descripcion = aportacion.Descripcion,

                };
                break;
            case ManageLlegadaBaseMedioDto llegadaBase:
                ejecucionPaso.LlegadaBaseMedio = new LlegadaBaseMedio
                {
                    IdCapacidad = llegadaBase.IdCapacidad,
                    MedioNoCatalogado = llegadaBase.MedioNoCatalogado,
                    FechaHoraLlegada = llegadaBase.FechaHoraLlegada,
                    Observaciones = llegadaBase.Observaciones
                };
                break;
        };
    }

    private async Task<Archivo?> MapArchivo(ManageSolicitudMedioDto manageSolicitudMedioDto, Archivo? archivoExistente)
    {
        if (manageSolicitudMedioDto.Archivo != null)
        {
            var fileEntity = new Archivo
            {
                NombreOriginal = manageSolicitudMedioDto.Archivo?.FileName ?? string.Empty,
                NombreUnico = $"{Path.GetFileNameWithoutExtension(manageSolicitudMedioDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{manageSolicitudMedioDto.Archivo?.Extension ?? string.Empty}",
                Tipo = manageSolicitudMedioDto.Archivo?.ContentType ?? string.Empty,
                Extension = manageSolicitudMedioDto.Archivo?.Extension ?? string.Empty,
                PesoEnBytes = manageSolicitudMedioDto.Archivo?.Length ?? 0,
            };

            fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(manageSolicitudMedioDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
            fileEntity.FechaCreacion = DateTime.Now;
            return fileEntity;
        }

        return archivoExistente;
    }

    private async Task ValidarFlujoPasos(ManageMovilizacionMediosCommand request)
    {
        // Obtener todo el flujo de la base de datos
        var flujoPasos = await _unitOfWork.Repository<FlujoPasoMovilizacion>().GetAllAsync();

        // Obtener el primer paso configurado (IdPasoActual = NULL)
        var primerPasoPermitido = flujoPasos.FirstOrDefault(f => f.IdPasoActual == null);
        if (primerPasoPermitido == null)
        {
            throw new BadRequestException("No se ha configurado un paso inicial en la base de datos.");
        }

        foreach (var movilizacion in request.Movilizaciones)
        {
            if (!movilizacion.Pasos.Any())
            {
                throw new BadRequestException($"La movilización con ID {movilizacion.Id} no contiene pasos.");
            }

            // Validar que el primer paso coincida con el configurado en la base de datos
            var primerPasoMovilizacion = movilizacion.Pasos.First();
            if ((int)primerPasoMovilizacion.TipoPaso != primerPasoPermitido.IdPasoSiguiente)
            {
                throw new BadRequestException(
                    $"El primer paso de la movilización con ID {movilizacion.Id} debe ser el paso {primerPasoPermitido.IdPasoSiguiente}."
                );
            }

            // Validar el flujo completo de pasos
            for (int i = 0; i < movilizacion.Pasos.Count - 1; i++)
            {
                var pasoActual = movilizacion.Pasos[i];
                var pasoSiguiente = movilizacion.Pasos[i + 1];

                var esPermitido = flujoPasos.Any(f =>
                    f.IdPasoActual == (int)pasoActual.TipoPaso &&
                    f.IdPasoSiguiente == (int)pasoSiguiente.TipoPaso);

                if (!esPermitido)
                {
                    throw new BadRequestException(
                        $"El paso {pasoActual.TipoPaso} no permite continuar al paso {pasoSiguiente.TipoPaso} en la movilización con ID {movilizacion.Id}."
                    );
                }
            }
        }

    }

    private async Task<ActuacionRelevanteDGPCE> GetOrCreateDocumentacionAsync(ManageMovilizacionMediosCommand request)
    {
        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante.Value > 0)
        {
            var spec = new ActuacionRelevanteDGPCESpecification(request.IdActuacionRelevante.Value);
            var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(spec);
            if (actuacionRelevante is null || actuacionRelevante.Borrado)
            {
                _logger.LogWarning($"request.IdActuacionRelevante: {request.IdActuacionRelevante}, no encontrado");
                throw new NotFoundException(nameof(ActuacionRelevanteDGPCE), request.IdActuacionRelevante);
            }
            return actuacionRelevante;
        }
        else
        {
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (suceso is null || suceso.Borrado)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            return new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
        }
    }

    private async Task ValidateIdsAsync(ManageMovilizacionMediosCommand request)
    {
        await ValidateIdProcedenciaMedioAsync(request);
        await ValidateIdDestinoMedioAsync(request);
        await ValidateIdTipoAdministracionMedioAsync(request);
        await ValidateIdCapacidadMedioAsync(request);
    }

    private async Task ValidateIdProcedenciaMedioAsync(ManageMovilizacionMediosCommand request)
    {
        // Extraer todos los IdProcedenciaMedio desde los pasos que son del tipo SolicitudMedioRequest
        var idsProcedenciaMedio = request.Movilizaciones
            .SelectMany(m => m.Pasos)
            .OfType<ManageSolicitudMedioDto>() // Filtra solo los pasos del tipo SolicitudMedioRequest
            .Select(p => p.IdProcedenciaMedio)
            .Distinct();

        // Obtener los Ids válidos desde la base de datos
        var procedenciaMedioExistentes = await _unitOfWork.Repository<ProcedenciaMedio>()
            .GetAsync(p => idsProcedenciaMedio.Contains(p.Id));

        if (procedenciaMedioExistentes.Count() != idsProcedenciaMedio.Count())
        {
            // Identificar los Ids inválidos
            var idsProcedenciaMedioExistentes = procedenciaMedioExistentes.Select(p => p.Id).ToList();
            var idsProcedenciaMedioInvalidos = idsProcedenciaMedio.Except(idsProcedenciaMedioExistentes).ToList();

            if (idsProcedenciaMedioInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia Medio no se encontraron: {string.Join(", ", idsProcedenciaMedioInvalidos)}");
                throw new NotFoundException(nameof(ProcedenciaMedio), string.Join(", ", idsProcedenciaMedioInvalidos));
            }
        }
    }

    private async Task ValidateIdDestinoMedioAsync(ManageMovilizacionMediosCommand request)
    {
        // Extraer todos los idsDestinoMedio desde los pasos que son del tipo ManageTramitacionMedioDto
        var idsDestinoMedio = request.Movilizaciones
            .SelectMany(m => m.Pasos)
            .OfType<ManageTramitacionMedioDto>()
            .Select(p => p.IdDestinoMedio)
            .Distinct();

        // Obtener los Ids válidos desde la base de datos
        var destinoMedioExistentes = await _unitOfWork.Repository<DestinoMedio>()
            .GetAsync(p => idsDestinoMedio.Contains(p.Id));

        if (destinoMedioExistentes.Count() != idsDestinoMedio.Count())
        {
            // Identificar los Ids inválidos
            var idsDestinoMedioExistentes = destinoMedioExistentes.Select(p => p.Id).ToList();
            var idsDestinoMedioInvalidos = idsDestinoMedio.Except(idsDestinoMedioExistentes).ToList();

            if (idsDestinoMedioInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia Medio no se encontraron: {string.Join(", ", idsDestinoMedioInvalidos)}");
                throw new NotFoundException(nameof(ProcedenciaMedio), string.Join(", ", idsDestinoMedioInvalidos));
            }
        }
    }

    private async Task ValidateIdTipoAdministracionMedioAsync(ManageMovilizacionMediosCommand request)
    {
        var idsTipoAdministracion = request.Movilizaciones
            .SelectMany(m => m.Pasos)
            .OfType<ManageAportacionMedioDto>()
            .Select(p => p.IdTipoAdministracion)
            .Distinct();

        // Obtener los Ids válidos desde la base de datos
        var tipoAdministracionExistentes = await _unitOfWork.Repository<TipoAdministracion>()
            .GetAsync(p => idsTipoAdministracion.Contains(p.Id));

        if (tipoAdministracionExistentes.Count() != idsTipoAdministracion.Count())
        {
            // Identificar los Ids inválidos
            var idsTipoAdministracionExistentes = tipoAdministracionExistentes.Select(p => p.Id).ToList();
            var idsTipoAdministracionInvalidos = idsTipoAdministracion.Except(idsTipoAdministracionExistentes).ToList();

            if (idsTipoAdministracionInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia Medio no se encontraron: {string.Join(", ", idsTipoAdministracionInvalidos)}");
                throw new NotFoundException(nameof(TipoAdministracion), string.Join(", ", idsTipoAdministracionInvalidos));
            }
        }
    }

    private async Task ValidateIdCapacidadMedioAsync(ManageMovilizacionMediosCommand request)
    {
        var idsCapacidad = request.Movilizaciones
            .SelectMany(m => m.Pasos)
            .Where(p => p is ManageAportacionMedioDto ||
                        p is ManageDespliegueMedioDto ||
                        p is ManageFinIntervencionMedioDto ||
                        p is ManageLlegadaBaseMedioDto)
            .Select(p => p switch
            {
                ManageAportacionMedioDto aportacion => (int?)aportacion.IdCapacidad,
                ManageDespliegueMedioDto despliegue => (int?)despliegue.IdCapacidad,
                ManageFinIntervencionMedioDto finIntervencion => (int?)finIntervencion.IdCapacidad,
                ManageLlegadaBaseMedioDto llegadaBase => (int?)llegadaBase.IdCapacidad,
                _ => null // Esto asegura que todos los casos devuelvan un int?
            })
            .Where(id => id.HasValue) // Filtra los valores nulos
            .Select(id => id.Value) // Convierte los Nullable<int> a int
            .Distinct();

        // Obtener los Ids válidos desde la base de datos
        var capacidadExistentes = await _unitOfWork.Repository<Capacidad>()
            .GetAsync(p => idsCapacidad.Contains(p.Id));

        if (capacidadExistentes.Count() != idsCapacidad.Count())
        {
            // Identificar los Ids inválidos
            var idsCapacidadExistentes = capacidadExistentes.Select(p => p.Id).ToList();
            var idsCapacidadInvalidos = idsCapacidad.Except(idsCapacidadExistentes).ToList();

            if (idsCapacidadInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia Medio no se encontraron: {string.Join(", ", idsCapacidadInvalidos)}");
                throw new NotFoundException(nameof(Capacidad), string.Join(", ", idsCapacidadInvalidos));
            }
        }
    }

    private async Task PersistDocumentacionAsync(ManageMovilizacionMediosCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        if (request.IdActuacionRelevante.HasValue && request.IdActuacionRelevante.Value > 0)
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacionRelevante);
        }
        else
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().AddEntity(actuacionRelevante);
        }

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar/actualizar los movimientos de medios de la Actuacion Relevante");
        }
    }

}
