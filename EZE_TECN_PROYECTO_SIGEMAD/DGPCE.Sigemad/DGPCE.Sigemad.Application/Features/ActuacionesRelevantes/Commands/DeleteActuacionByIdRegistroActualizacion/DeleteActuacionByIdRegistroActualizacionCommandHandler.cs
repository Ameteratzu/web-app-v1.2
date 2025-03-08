using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Commands.DeleteActuacionByIdRegistroActualizacion;
public class DeleteActuacionByIdRegistroActualizacionCommandHandler : IRequestHandler<DeleteActuacionByIdRegistroActualizacionCommand>
{
    private readonly ILogger<DeleteActuacionByIdRegistroActualizacionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<int> _idsEstadosCreados = new()
    {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.CreadoYModificado
    };

    public DeleteActuacionByIdRegistroActualizacionCommandHandler(
        ILogger<DeleteActuacionByIdRegistroActualizacionCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteActuacionByIdRegistroActualizacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN - {nameof(DeleteActuacionByIdRegistroActualizacionCommandHandler)}");

        RegistroActualizacion registro = await ObtenerRegistroActualizacion(request.IdRegistroActualizacion);
        ActuacionRelevanteDGPCE actuacion = await ObtenerActuacionRelevante(registro.IdSuceso);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            EliminarEntidadesRelacionadas(actuacion, registro);
            await EliminarRegistroActualizacion(registro);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de DeleteActuacionByIdRegistroActualizacionCommandHandler");
            throw;
        }

        _logger.LogInformation($"END - {nameof(DeleteActuacionByIdRegistroActualizacionCommandHandler)}");
        return Unit.Value;
    }

    private async Task<RegistroActualizacion> ObtenerRegistroActualizacion(int idRegistroActualizacion)
    {
        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams { Id = idRegistroActualizacion });
        var registro = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

        if (registro is null || registro.Borrado || registro.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.ActuacionRelevante)
        {
            _logger.LogWarning($"RegistroActualizacion no encontrado o inválido | IdRegistroActualizacion: {idRegistroActualizacion}");
            throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
        }

        return registro;
    }

    private async Task<ActuacionRelevanteDGPCE> ObtenerActuacionRelevante(int idSuceso)
    {
        var spec = new ActuacionRelevanteDGPCEActiveByIdSpecification(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = idSuceso });
        ActuacionRelevanteDGPCE actuacion = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(spec);

        if (actuacion is null || actuacion.Borrado)
        {
            _logger.LogWarning($"Actuacion Relevante no encontrada o inválida | IdSuceso: {idSuceso}");
            throw new NotFoundException(nameof(Evolucion), idSuceso);
        }

        return actuacion;
    }

    private async Task EliminarRegistroActualizacion(RegistroActualizacion registro)
    {
        foreach (var detalle in registro.DetallesRegistro)
        {
            await _unitOfWork.Repository<DetalleRegistroActualizacion>().DeleteAsync(detalle);
        }
        await _unitOfWork.Repository<RegistroActualizacion>().DeleteAsync(registro);
    }

    private bool DebeEliminar(RegistroActualizacion registro, ApartadoRegistroEnum apartado, int? idElemento)
    {
        return idElemento.HasValue && registro.DetallesRegistro
            .Any(dr => dr.IdApartadoRegistro == (int)apartado && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro) && dr.IdReferencia == idElemento.Value);
    }

    private void EliminarElementos<T>(List<T> lista, RegistroActualizacion registro, ApartadoRegistroEnum apartado, bool eliminarDetalles = false)
        where T : BaseDomainModel<int>
    {
        var idsAEliminar = registro.DetallesRegistro
            .Where(dr => dr.IdApartadoRegistro == (int)apartado && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
            .Select(dr => dr.IdReferencia)
            .ToList();

        var elementosEliminar = lista.Where(e => idsAEliminar.Contains(e.Id)).ToList();

        foreach (var elemento in elementosEliminar)
        {
            if (eliminarDetalles && elemento is MovilizacionMedio movilizacion)
            {
                foreach (var pasoAEliminar in movilizacion.Pasos)
                {
                    _unitOfWork.Repository<EjecucionPaso>().DeleteEntity(pasoAEliminar);
                }
            }
            _unitOfWork.Repository<T>().DeleteEntity(elemento);
        }
    }

    private void EliminarEntidadesRelacionadas(ActuacionRelevanteDGPCE actuacion, RegistroActualizacion registro)
    {
        EliminarElementos(actuacion.MovilizacionMedios, registro, ApartadoRegistroEnum.MovilizacionMedios, eliminarDetalles: true);
        EliminarElementos(actuacion.ConvocatoriasCECOD, registro, ApartadoRegistroEnum.ConvocatoriaCECOD);
        EliminarElementos(actuacion.ActivacionPlanEmergencias, registro, ApartadoRegistroEnum.ActivacionDePlanes);
        EliminarElementos(actuacion.NotificacionesEmergencias, registro, ApartadoRegistroEnum.NotificacionesOficiales);
        EliminarElementos(actuacion.ActivacionSistemas, registro, ApartadoRegistroEnum.ActivacionDeSistemas);
        EliminarElementos(actuacion.DeclaracionesZAGEP, registro, ApartadoRegistroEnum.DeclaracionZAGEP);

        if (DebeEliminar(registro, ApartadoRegistroEnum.EmergenciaNacional, actuacion.EmergenciaNacional?.Id))
            _unitOfWork.Repository<EmergenciaNacional>().DeleteEntity(actuacion.EmergenciaNacional);

        _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacion);
    }
}
