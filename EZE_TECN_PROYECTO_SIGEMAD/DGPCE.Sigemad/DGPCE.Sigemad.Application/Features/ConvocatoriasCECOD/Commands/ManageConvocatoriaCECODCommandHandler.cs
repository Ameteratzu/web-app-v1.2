﻿using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Direcciones.Commands.CreateDirecciones;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
public class ManageConvocatoriaCECODCommandHandler : IRequestHandler<ManageConvocatoriaCECODCommand, ManageConvocatoriaCECODResponse>
{
    private readonly ILogger<ManageConvocatoriaCECODCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageConvocatoriaCECODCommandHandler(
        ILogger<ManageConvocatoriaCECODCommandHandler> logger,
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
    public async Task<ManageConvocatoriaCECODResponse> Handle(ManageConvocatoriaCECODCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageConvocatoriaCECODCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<ActuacionRelevanteDGPCE>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.ActuacionRelevante);

            ActuacionRelevanteDGPCE actuacion = await GetOrCreateActuacionRelevante(request, registroActualizacion);

            var convocatoriasOriginales = actuacion.ConvocatoriasCECOD.ToDictionary(d => d.Id, d => _mapper.Map<ManageConvocatoriaCECODDto>(d));
            MapAndSaveDirecciones(request, actuacion);

            var convocatoriasParaEliminar = await DeleteLogicalDeclaraciones(request, actuacion, registroActualizacion.Id);

            await SaveDireccionAcuacion(actuacion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                ActuacionRelevanteDGPCE, ConvocatoriaCECOD, ManageConvocatoriaCECODDto>(
                registroActualizacion,
                actuacion,
                ApartadoRegistroEnum.ConvocatoriaCECOD,
                convocatoriasParaEliminar, convocatoriasOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCommandHandler)} - END");

            return new ManageConvocatoriaCECODResponse
            {
                IdActuacionRelevante = actuacion.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageConvocatoriaCECODCommandHandler");
            throw;
        }
    }



    private async Task SaveDireccionAcuacion(ActuacionRelevanteDGPCE actuacion)
    {
        if (actuacion.Id > 0)
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacion);
        }
        else
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().AddEntity(actuacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la ActuacionRelevanteDGPCE");
    }

    private async Task<List<int>> DeleteLogicalDeclaraciones(ManageConvocatoriaCECODCommand request, ActuacionRelevanteDGPCE actuacion, int idRegistroActualizacion)
    {
        if (actuacion.Id > 0)
        {
            var idsEnRequest = request.Detalles.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var convocatoriasCECODParaEliminar = actuacion.ConvocatoriasCECOD
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (convocatoriasCECODParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas convocatorias
            var idsConvocatoriasCECODParaEliminar = convocatoriasCECODParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsConvocatoriasCECODParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.ConvocatoriaCECOD);

            foreach (var declaracion in convocatoriasCECODParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == declaracion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La convocatoriaCECOD con ID {declaracion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<ConvocatoriaCECOD>().DeleteEntity(declaracion);
            }

            return idsConvocatoriasCECODParaEliminar;
        }

        return new List<int>();
    }


    private void MapAndSaveDirecciones(ManageConvocatoriaCECODCommand request, ActuacionRelevanteDGPCE actuacion)
    {
        foreach (var declaracionDto in request.Detalles)
        {
            if (declaracionDto.Id.HasValue && declaracionDto.Id > 0)
            {
                var declaracionExistente = actuacion.ConvocatoriasCECOD.FirstOrDefault(d => d.Id == declaracionDto.Id.Value);
                if (declaracionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageConvocatoriaCECODDto>(declaracionExistente);
                    var copiaNueva = _mapper.Map<ManageConvocatoriaCECODDto>(declaracionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(declaracionDto, declaracionExistente);
                        declaracionExistente.Borrado = false;
                    }
                }
                else
                {
                    actuacion.ConvocatoriasCECOD.Add(_mapper.Map<ConvocatoriaCECOD>(declaracionDto));
                }
            }
            else
            {
                actuacion.ConvocatoriasCECOD.Add(_mapper.Map<ConvocatoriaCECOD>(declaracionDto));
            }
        }
    }


    private async Task<ActuacionRelevanteDGPCE> GetOrCreateActuacionRelevante(ManageConvocatoriaCECODCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            bool includeEmergenciaNacional = false;
            List<int> idsActivacionPlanEmergencias = new List<int>();
            List<int> idsDeclaracionesZAGEP = new List<int>();
            List<int> idsActivacionSistemas = new List<int>();
            List<int> idsConvocatoriasCECOD = new List<int>();
            List<int> idsNotificacionesEmergencias = new List<int>();
            List<int> idsMovilizacionMedios = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.EmergenciaNacional:
                        includeEmergenciaNacional = true;
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDePlanes:
                        idsActivacionPlanEmergencias.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.DeclaracionZAGEP:
                        idsDeclaracionesZAGEP.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDeSistemas:
                        idsActivacionSistemas.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.ConvocatoriaCECOD:
                        idsConvocatoriasCECOD.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.NotificacionesOficiales:
                        idsNotificacionesEmergencias.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.MovilizacionMedios:
                        idsMovilizacionMedios.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Dirección y Coordinación de Emergencia por IdReferencia
            var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
                .GetByIdWithSpec(new ActuacionRelevanteDGPCEWithFilteredData(registroActualizacion.IdReferencia, idsActivacionPlanEmergencias, idsDeclaracionesZAGEP, idsActivacionSistemas, idsConvocatoriasCECOD, idsNotificacionesEmergencias, idsMovilizacionMedios, includeEmergenciaNacional));

            if (actuacionRelevante is null || actuacionRelevante.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de actuacionRelevanteDGPCE");

            return actuacionRelevante;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new ActuacionRelevanteDGPCEWithEmergenciaNacional(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = request.IdSuceso });
        var emergenciaNacionalExistente = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(specSuceso);

        return emergenciaNacionalExistente ?? new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
    }

}
