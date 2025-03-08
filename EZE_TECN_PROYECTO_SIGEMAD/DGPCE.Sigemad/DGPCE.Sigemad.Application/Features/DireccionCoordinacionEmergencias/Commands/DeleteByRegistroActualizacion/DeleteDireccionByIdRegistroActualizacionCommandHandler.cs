using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.DireccionCoordinacionEmergencias;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.DeleteByRegistroActualizacion;
public class DeleteDireccionByIdRegistroActualizacionCommandHandler : IRequestHandler<DeleteDireccionByIdRegistroActualizacionCommand>
{
    private readonly ILogger<DeleteDireccionByIdRegistroActualizacionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<int> _idsEstadosCreados = new()
    {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.CreadoYModificado
    };

    public DeleteDireccionByIdRegistroActualizacionCommandHandler(
        ILogger<DeleteDireccionByIdRegistroActualizacionCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteDireccionByIdRegistroActualizacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN - {nameof(DeleteDireccionByIdRegistroActualizacionCommand)}");

        RegistroActualizacion registro = await ObtenerRegistroActualizacion(request.IdRegistroActualizacion);
        DireccionCoordinacionEmergencia direccionCoordinacion = await ObtenerDireccionCoordincion(registro.IdSuceso);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            EliminarEntidadesRelacionadas(direccionCoordinacion, registro);
            await EliminarRegistroActualizacion(registro);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de DeleteDireccionByIdRegistroActualizacionCommand");
            throw;
        }

        _logger.LogInformation($"END - {nameof(DeleteDireccionByIdRegistroActualizacionCommand)}");
        return Unit.Value;
    }

    private async Task<RegistroActualizacion> ObtenerRegistroActualizacion(int idRegistroActualizacion)
    {
        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams { Id = idRegistroActualizacion });
        var registro = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

        if (registro is null || registro.Borrado || registro.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.DireccionCoordinacion)
        {
            _logger.LogWarning($"RegistroActualizacion no encontrado o inválido | IdRegistroActualizacion: {idRegistroActualizacion}");
            throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
        }

        return registro;
    }

    private async Task<DireccionCoordinacionEmergencia> ObtenerDireccionCoordincion(int idSuceso)
    {
        var spec = new DireccionCoordinacionEmergenciaActiveByIdSpecification(new DireccionCoordinacionEmergenciaSpecificationParams { IdSuceso = idSuceso });
        var direccion = await _unitOfWork.Repository<DireccionCoordinacionEmergencia>().GetByIdWithSpec(spec);

        if (direccion is null || direccion.Borrado)
        {
            _logger.LogWarning($"Direccion y Coordinacion no encontrada o inválida | IdSuceso: {idSuceso}");
            throw new NotFoundException(nameof(DireccionCoordinacionEmergencia), idSuceso);
        }

        return direccion;
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
            //if (eliminarDetalles && elemento is IntervencionMedio intervencion)
            //{
            //    foreach (var detalle in intervencion.DetalleIntervencionMedios)
            //    {
            //        _unitOfWork.Repository<DetalleIntervencionMedio>().DeleteEntity(detalle);
            //    }
            //}
            _unitOfWork.Repository<T>().DeleteEntity(elemento);
        }
    }

    private void EliminarEntidadesRelacionadas(DireccionCoordinacionEmergencia direccionCoordinacion, RegistroActualizacion registro)
    {
        EliminarElementos(direccionCoordinacion.Direcciones, registro, ApartadoRegistroEnum.Direccion);
        EliminarElementos(direccionCoordinacion.CoordinacionesPMA, registro, ApartadoRegistroEnum.CoordinacionPMA);
        EliminarElementos(direccionCoordinacion.CoordinacionesCecopi, registro, ApartadoRegistroEnum.CoordinacionCECOPI);

        _unitOfWork.Repository<DireccionCoordinacionEmergencia>().UpdateEntity(direccionCoordinacion);
    }
}
